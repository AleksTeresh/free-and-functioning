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
    protected string[] actions = { };
    protected bool currentlySelected = false;
    protected Bounds selectionBounds;
    protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    protected GUIStyle healthStyle = new GUIStyle();
    protected float healthPercentage = 1.0f;

    protected WorldObject target = null;
    protected bool attacking = false;
    public float weaponRange = 10.0f;
    protected bool movingIntoPosition = false;
    protected bool aiming = false;
    public float weaponAimSpeed = 1.0f;
    public float weaponRechargeTime = 1.0f;
    private float currentWeaponChargeTime;

    // loading related
    protected bool loadedSavedValues = false;
    private int loadedTargetId = -1;

    // audio related
    public AudioClip attackSound, selectSound, useWeaponSound;
    public float attackVolume = 1.0f, selectVolume = 1.0f, useWeaponVolume = 1.0f;

    // AI related
    public float detectionRange = 20.0f;
    protected List<WorldObject> nearbyObjects;

    //we want to restrict how many decisions are made to help with game performance
    //the default time at the moment is a tenth of a second
    private float timeSinceLastDecision = 0.0f, timeBetweenDecisions = 0.1f;

    protected AudioElement audioElement;

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
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
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
        SaveManager.WriteBoolean(writer, "Attacking", attacking);
        SaveManager.WriteBoolean(writer, "MovingIntoPosition", movingIntoPosition);
        SaveManager.WriteBoolean(writer, "Aiming", aiming);
        if (attacking)
        {
            //only save if attacking so that we do not end up storing massive numbers for no reason
            SaveManager.WriteFloat(writer, "CurrentWeaponChargeTime", currentWeaponChargeTime);
        }
        if (target != null) SaveManager.WriteInt(writer, "TargetId", target.ObjectId);
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

    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle input if currently selected
        if (currentlySelected && !WorkManager.ObjectIsGround(hitObject))
        {
            WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
            //clicked on another selectable object
            if (worldObject)
            {
                Player owner = hitObject.transform.root.GetComponent<Player>();
                if (owner)
                { //the object is controlled by a player
                    if (player && player.human)
                    { //this object is controlled by a human player
                      //start attack if object is not owned by the same player and this object can attack, else select
                        if (player.username != owner.username && CanAttack()) BeginAttack(worldObject);
                        else ChangeSelection(worldObject, controller);
                    }
                    else ChangeSelection(worldObject, controller);
                }
                else ChangeSelection(worldObject, controller);
            }
        }
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
                    if (owner.username == player.username) player.hud.SetCursorState(CursorState.Select);
                    else if (CanAttack()) player.hud.SetCursorState(CursorState.Attack);
                    else player.hud.SetCursorState(CursorState.Select);
                }
                else if (unit || building && CanAttack()) player.hud.SetCursorState(CursorState.Attack);
                else player.hud.SetCursorState(CursorState.Select);
            }
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    protected virtual void Awake()
    {
        selectionBounds = ResourceManager.InvalidBounds;
        CalculateBounds();
    }

    protected virtual void Start()
    {
        SetPlayer();
        if (player)
        {
            if (loadedSavedValues)
            {
                if (loadedTargetId >= 0) target = player.GetObjectForId(loadedTargetId);
            }
            else
            {
                SetTeamColor();
            }
        }

        InitialiseAudio();
    }

    protected virtual void Update()
    {
        if (ShouldMakeDecision()) DecideWhatToDo();

        currentWeaponChargeTime += Time.deltaTime;
        if (attacking && !movingIntoPosition && !aiming) PerformAttack();
    }

    protected virtual void OnGUI()
    {
        if (currentlySelected && !ResourceManager.MenuOpen) DrawSelection();
    }

    /**
     * A child class should only determine other conditions under which a decision should
     * not be made. This could be 'harvesting' for a harvester, for example. Alternatively,
     * an object that never has to make decisions could just return false.
     */
    protected virtual bool ShouldMakeDecision()
    {
        if (!attacking && !movingIntoPosition && !aiming)
        {
            //we are not doing anything at the moment
            if (timeSinceLastDecision > timeBetweenDecisions)
            {
                timeSinceLastDecision = 0.0f;
                return true;
            }
            timeSinceLastDecision += Time.deltaTime;
        }
        return false;
    }

    protected virtual void DecideWhatToDo()
    {
        //determine what should be done by the world object at the current point in time
        Vector3 currentPosition = transform.position;
        nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, detectionRange);

        if (CanAttack())
        {
            List<WorldObject> enemyObjects = new List<WorldObject>();
            foreach (WorldObject nearbyObject in nearbyObjects)
            {
                // Resource resource = nearbyObject.GetComponent<Resource>();
                // if (resource) continue;
                if (nearbyObject.GetPlayer() != player) enemyObjects.Add(nearbyObject);
            }
            WorldObject closestObject = WorkManager.FindNearestWorldObjectInListToPosition(enemyObjects, currentPosition);
            if (closestObject) BeginAttack(closestObject);
        }
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

    protected virtual void BeginAttack(WorldObject target)
    {
        if (audioElement != null) audioElement.Play(attackSound);

        this.target = target;
        if (TargetInRange())
        {
            attacking = true;
            PerformAttack();
        }
        else AdjustPosition();
    }

    public virtual bool CanAttack()
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
            case "Attacking": attacking = (bool)readValue; break;
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

    private void ChangeSelection(WorldObject worldObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        SetSelection(false, playingArea);
        if (controller.SelectedObject) controller.SelectedObject.SetSelection(false, playingArea);
        controller.SelectedObject = worldObject;
        worldObject.SetSelection(true, controller.hud.GetPlayingArea());
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

    public void TakeDamage(int damage)
    {
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

    private bool TargetInRange()
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - transform.position;
        if (direction.sqrMagnitude < weaponRange * weaponRange)
        {
            return true;
        }
        return false;
    }

    private void AdjustPosition()
    {
        Unit self = this as Unit;
        if (self)
        {
            movingIntoPosition = true;
            Vector3 attackPosition = FindNearestAttackPosition();
            self.StartMove(attackPosition);
            attacking = true;
        }
        else attacking = false;
    }

    private Vector3 FindNearestAttackPosition()
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - transform.position;
        float targetDistance = direction.magnitude;
        float distanceToTravel = targetDistance - (0.9f * weaponRange);
        return Vector3.Lerp(transform.position, targetLocation, distanceToTravel / targetDistance);
    }

    private bool TargetInFrontOfWeapon()
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - transform.position;
        if (WorkManager.V3Equal(direction.normalized, transform.forward.normalized)) return true;
        else return false;
    }

    protected virtual void AimAtTarget()
    {
        aiming = true;
        //this behaviour needs to be specified by a specific object
    }

    private bool ReadyToFire()
    {
        if (currentWeaponChargeTime >= weaponRechargeTime) return true;
        return false;
    }

    protected virtual void UseWeapon()
    {
        if (audioElement != null && Time.timeScale > 0) audioElement.Play(useWeaponSound);

        currentWeaponChargeTime = 0.0f;
        //this behaviour needs to be specified by a specific object
    }


    private void PerformAttack()
    {
        if (!target)
        {
            attacking = false;
            return;
        }
        if (!TargetInRange()) AdjustPosition();
        else if (!TargetInFrontOfWeapon()) AimAtTarget();
        else if (ReadyToFire()) UseWeapon();
    }
}
