using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class WorldObject : MonoBehaviour {

    public int ObjectId { get; set; }
    public string objectName;
    public Texture2D buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;

    protected Player player;
    protected HUD hud;
    protected string[] actions = { };
    protected bool currentlySelected = false;
    protected Bounds selectionBounds;
    protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    protected GUIStyle healthStyle = new GUIStyle();
    protected float healthPercentage = 1.0f;

    public float weaponRange = 10.0f;
    protected bool movingIntoPosition = false;
    protected bool aiming = false;
    public float weaponAimSpeed = 1.0f;
    public float weaponRechargeTime = 1.0f;
    public float weaponMultiRechargeTime = 1.0f;
    private float currentWeaponChargeTime;
    private float currentWeaponMultiChargeTime;

    // loading related
    protected bool loadedSavedValues = false;
    private int loadedTargetId = -1;

    // audio related
    public AudioClip attackSound, selectSound, useWeaponSound;
    public float attackVolume = 1.0f, selectVolume = 1.0f, useWeaponVolume = 1.0f;
    protected AudioElement audioElement;

    // AI related
    public float detectionRange = 20.0f;
    protected StateController stateController;
    private int underAttackFrameCounter;

    // child renderers without Particle system
    private List<Renderer> childRenderersWithoutParticles;

    // Fog Of War
    private FogOfWarAgent fogOfWarAgent;

    public virtual void SetSelection(bool selected, Rect playingArea)
    {
        currentlySelected = selected;
        if (selected)
        {
            if (audioElement != null) audioElement.Play(selectSound);

            this.playingArea = playingArea;
        }
        
    }

    public string[] GetActions()
    {
        return actions;
    }

    public void CalculateBounds()
    {
        selectionBounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in childRenderersWithoutParticles)
        {
            selectionBounds.Encapsulate(r.bounds);
        }
    }

    public void SetPlayer()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }

    public virtual void SaveDetails(JsonWriter writer)
    {
        SaveManager.WriteString(writer, "Type", name);
        SaveManager.WriteString(writer, "Name", objectName);
        SaveManager.WriteInt(writer, "Id", ObjectId);
        SaveManager.WriteVector(writer, "Position", transform.position);
        SaveManager.WriteQuaternion(writer, "Rotation", transform.rotation);
        SaveManager.WriteVector(writer, "Scale", transform.localScale);
        SaveManager.WriteInt(writer, "HitPoints", hitPoints);
        // SaveManager.WriteBoolean(writer, "Attacking", attacking);
        SaveManager.WriteBoolean(writer, "MovingIntoPosition", movingIntoPosition);
        SaveManager.WriteBoolean(writer, "Aiming", aiming);
 
        // if (target != null) SaveManager.WriteInt(writer, "TargetId", target.ObjectId);
    }

    public void LoadDetails(JsonTextReader reader)
    {
        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;
                    reader.Read();
                    HandleLoadedProperty(reader, propertyName, reader.Value);
                }
            }
            else if (reader.TokenType == JsonToken.EndObject)
            {
                //loaded position invalidates the selection bounds so they must be recalculated
                selectionBounds = ResourceManager.InvalidBounds;
                CalculateBounds();
                loadedSavedValues = true;
                return;
            }
        }
        //loaded position invalidates the selection bounds so they must be recalculated
        selectionBounds = ResourceManager.InvalidBounds;
        CalculateBounds();
        loadedSavedValues = true;
    }



    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
    }

    public virtual void SetHoverState(GameObject hoverObject)
    {
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            //something other than the ground is being hovered over
            if (!WorkManager.ObjectIsGround(hoverObject))
            {
                Player owner = hoverObject.transform.root.GetComponent<Player>();
                Unit unit = hoverObject.transform.parent.GetComponent<Unit>();
                Building building = hoverObject.transform.parent.GetComponent<Building>();
                if (owner)
                { //the object is owned by a player
                    if (owner.username == player.username) hud.SetCursorState(CursorState.Select);
                    else if (CanAttack()) hud.SetCursorState(CursorState.Attack);
                    else hud.SetCursorState(CursorState.Select);
                }
                else if (unit || building && CanAttack()) hud.SetCursorState(CursorState.Attack);
                else hud.SetCursorState(CursorState.Select);
            }
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    public FogOfWarAgent GetFogOfWarAgent()
    {
        return fogOfWarAgent;
    }

    public StateController GetStateController()
    {
        return stateController;
    }

    public bool IsSelected ()
    {
        return currentlySelected;
    }

    public bool IsUnderAttack()
    {
        return underAttackFrameCounter > 0;
    }

    public void UpdateChildRenderers()
    {
        // retrieve child renderers
        var renderers = GetComponentsInChildren<Renderer>();
        childRenderersWithoutParticles = new List<Renderer>();
        // filter out particle system renderers
        foreach (Renderer r in renderers)
        {
            if (r.enabled && r.GetComponentInParent<ParticleSystem>() == null)
            {
                childRenderersWithoutParticles.Add(r);
            }
        }
    }

    protected virtual void Awake()
    {
        fogOfWarAgent = GetComponent<FogOfWarAgent>();

        selectionBounds = ResourceManager.InvalidBounds;

        UpdateChildRenderers();

        CalculateBounds();

        stateController = GetComponent<StateController>();
    }


    protected virtual void Start()
    {
        underAttackFrameCounter = 0;

        SetPlayer();
        if (player)
        {
            hud = player.GetComponentInChildren<HUD>();

            if (loadedSavedValues)
            {
                // if (loadedTargetId >= 0) target = player.GetObjectForId(loadedTargetId);
            }
            else
            {
                SetTeamColor();
            }
        }

        InitialiseAudio();
        // enable AI by default, if possible
        if (stateController)
        {
            stateController.SetupAI(true);
        }
    }

    protected virtual void Update()
    {
        currentWeaponChargeTime += Time.deltaTime;
        underAttackFrameCounter = Mathf.Max(0, underAttackFrameCounter - 1);

        if (CanAttackMulti())
        {
            currentWeaponMultiChargeTime += Time.deltaTime;
        }
    }

    protected virtual void OnGUI()
    {
        if (currentlySelected && !ResourceManager.MenuOpen) DrawSelection();
    }

    protected virtual void InitialiseAudio()
    {
        List<AudioClip> sounds = new List<AudioClip>();
        List<float> volumes = new List<float>();
        if (attackVolume < 0.0f) attackVolume = 0.0f;
        if (attackVolume > 1.0f) attackVolume = 1.0f;
        sounds.Add(attackSound);
        volumes.Add(attackVolume);
        if (selectVolume < 0.0f) selectVolume = 0.0f;
        if (selectVolume > 1.0f) selectVolume = 1.0f;
        sounds.Add(selectSound);
        volumes.Add(selectVolume);
        if (useWeaponVolume < 0.0f) useWeaponVolume = 0.0f;
        if (useWeaponVolume > 1.0f) useWeaponVolume = 1.0f;
        sounds.Add(useWeaponSound);
        volumes.Add(useWeaponVolume);
        audioElement = new AudioElement(sounds, volumes, objectName + ObjectId, this.transform);
    }

    public virtual void PerformAttack(WorldObject target)
    {
        if (!target)
        {
            // attacking = false;
            return;
        }
        if (!TargetInFrontOfWeapon(target)) AimAtTarget(target);
        else if (ReadyToFire()) UseWeapon(target);
    }

    public virtual void PerformAttackToMulti(List<WorldObject> targets)
    {
        if (targets == null || targets.Count == 0)
        {
            return;
        }

        if (ReadyToFireMulti()) UseWeaponMulti(targets);
    }

    public virtual void BeginAttackToMulti(List<WorldObject> targets)
    {
        if (CanAttackMulti())
        {
            if (audioElement != null) audioElement.Play(attackSound);
            // this.target = null;
        }
        else
        {
            // BeginAttack(target);
        }
    }

    public virtual bool CanAttack()
    {
        //default behaviour needs to be overidden by children
        return false;
    }

    public virtual bool CanAttackMulti()
    {
        //default behaviour needs to be overidden by children
        return false;
    }

    protected virtual void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        switch (propertyName)
        {
            case "Name": objectName = (string)readValue; break;
            case "Id": ObjectId = (int)(System.Int64)readValue; break;
            case "Position": transform.localPosition = LoadManager.LoadVector(reader); break;
            case "Rotation": transform.localRotation = LoadManager.LoadQuaternion(reader); break;
            case "Scale": transform.localScale = LoadManager.LoadVector(reader); break;
            case "HitPoints": hitPoints = (int)(System.Int64)readValue; break;
            // case "Attacking": attacking = (bool)readValue; break;
            case "MovingIntoPosition": movingIntoPosition = (bool)readValue; break;
            case "Aiming": aiming = (bool)readValue; break;
            case "CurrentWeaponChargeTime": currentWeaponChargeTime = (float)(double)readValue; break;
            case "TargetId": loadedTargetId = (int)(System.Int64)readValue; break;
            default: break;
        }
    }

    protected virtual void DrawSelectionBox(Rect selectBox)
    {
        GUI.Box(selectBox, "");
        CalculateCurrentHealth();
        GUI.Label(new Rect(selectBox.x, selectBox.y - 7, selectBox.width * healthPercentage, 5), "", healthStyle);
    }

    private void DrawSelection()
    {
        GUI.skin = ResourceManager.SelectBoxSkin;
        Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
        //Draw the selection box around the currently selected object, within the bounds of the playing area
        GUI.BeginGroup(playingArea);
        DrawSelectionBox(selectBox);
        GUI.EndGroup();
    }

    public bool IsOwnedBy(Player owner)
    {
        if (player && player.Equals(owner))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        underAttackFrameCounter = 2;
        hitPoints -= damage;
        if (hitPoints <= 0) Destroy(gameObject);
    }

    public Bounds GetSelectionBounds()
    {
        return selectionBounds;
    }

    protected virtual void CalculateCurrentHealth()
    {
        healthPercentage = (float)hitPoints / (float)maxHitPoints;
        if (healthPercentage > 0.65f) healthStyle.normal.background = ResourceManager.HealthyTexture;
        else if (healthPercentage > 0.35f) healthStyle.normal.background = ResourceManager.DamagedTexture;
        else healthStyle.normal.background = ResourceManager.CriticalTexture;
    }

    public void SetTeamColor()
    {
        TeamColor[] teamColors = GetComponentsInChildren<TeamColor>();
        foreach (TeamColor teamColor in teamColors) teamColor.GetComponent<Renderer>().material.color = player.teamColor;
    }

    private bool TargetInFrontOfWeapon(WorldObject target)
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - transform.position;

        // ignore height when considering 
        var a = new Vector3(direction.normalized.x, 0, direction.normalized.z);
        var b = new Vector3(transform.forward.normalized.x, 0, transform.forward.normalized.z);

        if (WorkManager.V3Equal(a, b)) return true;
        else return false;
    }

    protected virtual void AimAtTarget(WorldObject target)
    {
        aiming = true;
        //this behaviour needs to be specified by a specific object
    }

    protected virtual void FireProjectile(WorldObject target, string projectileName, Vector3 spawnPoint)
    {
        FireProjectile(target, projectileName, spawnPoint, transform.rotation);
    }

    protected virtual void FireProjectile(WorldObject target, string projectileName, Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetWorldObject(projectileName), spawnPoint, rotation);
        Projectile projectile = gameObject.GetComponentInChildren<Projectile>();
        projectile.Player = this.player;
        projectile.SetRange(weaponRange);
        projectile.SetTarget(target);
    }

    private bool ReadyToFire()
    {
        if (currentWeaponChargeTime >= weaponRechargeTime) return true;
        return false;
    }

    private bool ReadyToFireMulti()
    {
        if (currentWeaponMultiChargeTime >= weaponMultiRechargeTime) return true;
        return false;
    }

    protected virtual void UseWeapon(WorldObject target)
    {
        if (audioElement != null && Time.timeScale > 0) audioElement.Play(useWeaponSound);

        currentWeaponChargeTime = 0.0f;
        //this behaviour needs to be specified by a specific object
    }

    protected virtual void UseWeaponMulti(List<WorldObject> target)
    {
        if (audioElement != null && Time.timeScale > 0) audioElement.Play(useWeaponSound);

        currentWeaponMultiChargeTime = 0.0f;
        //this behaviour needs to be specified by a specific object
    }
}
