using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;
using Abilities;
using Statuses;
using System;
using Persistence;

public class WorldObject : MonoBehaviour {
    public int ObjectId { get; set; }
    public string objectName;
    // public Texture2D buildImage;
    // public int cost, sellValue,
    public int hitPoints;
    public int maxHitPoints;

    // an object is busy and does not react to any commands
    protected bool isBusy;

    // child renderers without Particle system
    private List<Renderer> objectModelChildRenderers;

    [Header("Status -related")]
    private Statuses.Statuses statusesWrapper;
    public List<Status> ActiveStatuses { get; private set; }

    [NonSerialized]
    protected Player player;
    [NonSerialized]
    protected HUD hud;
    [NonSerialized]
    protected TargetManager targetManager;
    protected string[] actions = { };
    protected bool currentlySelected = false;
    private Light selectionLight;
    private Light targetLight;
    [NonSerialized]
    protected Bounds selectionBounds;
    [NonSerialized]
    protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    // protected GUIStyle healthStyle = new GUIStyle();
    // protected float healthPercentage = 1.0f;

    [Header("Weapon and attack")]

    public float attackDelay = 1;
    public float attackMultiDelay = 1;
    public int damage = 10;
    public int damageMulti = 3;
    public float weaponRange = 10.0f;
    protected bool movingIntoPosition = false;
    public bool aiming = false;
    public float weaponAimSpeed = 1.0f;
    public float weaponRechargeTime = 1.0f;
    public float weaponMultiRechargeTime = 1.0f;
    protected Quaternion aimRotation;
    protected int attackDelayFrameCounter;
    private float currentWeaponChargeTime;
    private float currentWeaponMultiChargeTime;
    private float currentAttackDelayTime = 0;
    private int chasedFrameCounter;

    [Header("Defence and Invincibility")]
    public float meleeDefence = 0;
    public float rangeDefence = 0;
    public float abilityDefence = 0;
    public bool isInvincible = false;

    [Header("Game Loading")]
    protected bool loadedSavedValues = false;
    private int loadedTargetId = -1;

    [Header("Audio")]
    public AudioClip attackSound, selectSound, useWeaponSound;
    public float attackVolume = 1.0f, selectVolume = 1.0f, useWeaponVolume = 1.0f;
    protected AudioElement audioElement;

    [Header("AI related")]
    public float detectionRange = 20.0f;
    protected StateController stateController;
    protected int underAttackFrameCounter;

    [Header("Fog of War")]
    public bool belongsToBoss;
    private FogOfWarAgent fogOfWarAgent;

    private Animator animator;

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
        selectionBounds = WorkManager.GetBounds(this.transform, objectModelChildRenderers);
    }

    public void SetPlayer()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }
    /*
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

        */

    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
    }

    public virtual void SetHoverState(GameObject hoverObject)
    {
        if (!hoverObject) return;

        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            //something other than the ground is being hovered over
            if (!WorkManager.ObjectIsGround(hoverObject) && hoverObject.transform.parent)
            {
                Player owner = hoverObject.transform.root.GetComponent<Player>();
                Unit unit = hoverObject.transform.parent.GetComponent<Unit>();
                Building building = hoverObject.transform.parent.GetComponent<Building>();
                BossPart bossPart = hoverObject.transform.parent.GetComponent<BossPart>();

                if (owner)
                { //the object is owned by a player
                    if (owner.username == player.username) hud.SetCursorState(CursorState.Select);
                    else if (unit || building || bossPart && CanAttack()) hud.SetCursorState(CursorState.Attack);
                    else hud.SetCursorState(CursorState.Select);
                }
                else if (unit || building || bossPart && CanAttack()) hud.SetCursorState(CursorState.Attack);
                else hud.SetCursorState(CursorState.Select);
            }
        }
    }

    public virtual bool CanAddStatus()
    {
        // override this method
        return false;
    }

    public void AddStatus(Status status)
    {
        // if the object does not have status wrapper
        if (!statusesWrapper)
        {
            return;
        }

        if (IsStatusActive(status))
        {
            var existingStatus = ActiveStatuses.Find(p => p.statusName == status.statusName);
            existingStatus.isActive = true;
            existingStatus.duration = 0;
        }
        else // if there is no status of this type, add it
        {
            ActiveStatuses.Add(status);
            status.transform.parent = statusesWrapper.transform;
        }        
    }

    public bool IsStatusActive(Status status)
    {
        ActiveStatuses = ActiveStatuses.Where(p => p != null).ToList();

        bool isActive = ActiveStatuses.Select(p => p.statusName).ToList().Contains(status.statusName);

        return isActive;
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

    public bool IsAttacking()
    {
        return attackDelayFrameCounter > 0;
    }

    public void UpdateChildRenderers()
    {
        objectModelChildRenderers = WorkManager.GetChildRenderers(this.transform);
    }

    public void ResetCurrentWeaponChargeTime()
    {
        this.currentWeaponChargeTime = 0;
    }

    public void ResetCurrentWeaponMultiChargeTime()
    {
        this.currentWeaponMultiChargeTime = 0;
    }

    public virtual void MarkAsChasedTarget()
    {
       chasedFrameCounter = 2;
       if (targetLight != null)
       {
            targetLight.enabled = true;
       }
    }

    protected virtual void Awake()
    {
        fogOfWarAgent = GetComponent<FogOfWarAgent>();

        selectionBounds = ResourceManager.InvalidBounds;

        UpdateChildRenderers();

        CalculateBounds();

        stateController = GetComponent<StateController>();

        selectionLight = GetComponentInChildren<Light>();

        ActiveStatuses = new List<Status>(GetComponentsInChildren<Status>());
        statusesWrapper = GetComponentInChildren<Statuses.Statuses>();

        var humanPlayer = PlayerManager.GetHumanPlayer(FindObjectsOfType<Player>());
        if (humanPlayer)
        {
            targetManager = humanPlayer.GetComponentInChildren<TargetManager>();
        }
    }

    protected virtual void Start()
    {
        // AwakeObj();

        underAttackFrameCounter = 0;
        attackDelayFrameCounter = 0;

        SetPlayer();
        if (player)
        {
            hud = player.GetComponentInChildren<HUD>();
            playingArea = hud.GetPlayingArea();

            if (loadedSavedValues)
            {
                // if (loadedTargetId >= 0) target = player.GetObjectById(loadedTargetId);
            }
            else
            {
                SetTeamColor();
            }

            if (!GetPlayer().human)
            {
                var targetLightObj = Instantiate(ResourceManager.GetVfx("TargetIndicator"), transform);
                targetLightObj.transform.parent = transform;
                targetLight = targetLightObj.GetComponent<Light>();
            }
        }

        InitialiseAudio();
        // enable AI by default, if possible
        if (stateController)
        {
            stateController.SetupAI(true);
        }

        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        currentWeaponChargeTime += Time.deltaTime;
        underAttackFrameCounter = Mathf.Max(0, underAttackFrameCounter - 1);

        chasedFrameCounter = Mathf.Max(0, chasedFrameCounter - 1);

        attackDelayFrameCounter = Mathf.Max(0, attackDelayFrameCounter - 1);
        if (IsAttacking())
        {
            currentAttackDelayTime += Time.deltaTime;
        }
        else
        {
            currentAttackDelayTime = 0;
        }

        if (CanAttackMulti())
        {
            currentWeaponMultiChargeTime += Time.deltaTime;
        }

        RemoveInactiveStatuses();

        HandleSelectionLight();

        HandleTargetIndicator();

        HandleAnimation();
    }

    protected virtual void OnDrawGizmosSelected ()
    {
        // detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // weapon range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponRange);
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
        if (!target || isBusy)
        {
            // attacking = false;
            return;
        }

        if (!TargetInFrontOfWeapon(target)) AimAtTarget(target);
        else if (!ReadyToFire()) return;
        else if (!AttackDelayIsOver())
        {
            attackDelayFrameCounter = 2;
        }
        else
        {
            UseWeapon(target);
        }
    }

	public virtual void UseAbility(WorldObject target, Ability ability, bool shouldAim = false) {
		if (!target || isBusy)
		{
			// attacking = false;
			return;
		}

		if (shouldAim && !TargetInFrontOfWeapon (target) && !IsTargetSelf(target)) {
			AimAtTarget (target);
		}
		else if (ability.IsReady()) {
			ability.Use(target);
		}
	}

    public virtual void UseAbility(List<WorldObject> targets, Ability ability)
    {
        if (targets == null || targets.Count == 0 || isBusy)
        {
            return;
        }

        if (ability.IsReady())
        {
            ability.Use(targets);
        }
    }

    public virtual void UseAbility(Vector3 position, AoeAbility ability)
    {
        if (!isBusy && ability.IsReady())
        {
            ability.Use(position);
        }
    }

    public void SetAimRotation(Quaternion rotation)
    {
        this.aimRotation = rotation;
    }

    public virtual void PerformAttackToMulti(List<WorldObject> targets)
    {
        if (targets == null || targets.Count == 0 || isBusy)
        {
            return;
        }

        if (!MultiAttackDelayIsOver()) attackDelayFrameCounter = 2;
        else if (ReadyToFireMulti()) UseWeaponMulti(targets);
    }
    /*
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
    */
    public virtual bool CanAttack()
    {
        //default behaviour needs to be overidden by children
        return false;
    }

	public virtual bool IsHealer() 
	{
		// Necessary for HotkeyUnitSelector, to check if the indicatedObject is healer
		return false;
	}
		

    public virtual bool CanAttackMulti()
    {
        //default behaviour needs to be overidden by children
        return false;
    }

    public virtual Vector3 GetProjectileSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;

        return spawnPoint;
    }

    /*
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
    }  */

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

    public virtual void TakeDamage(int attackPoints, AttackType attackType)
    {
        float damage = 0;

        if (attackPoints == 0 || isInvincible)
        {
            damage = 0;
        }
        else
        {
            switch (attackType)
            {
                case AttackType.Melee:
                    damage = attackPoints * attackPoints / (attackPoints + meleeDefence);
                    break;

                case AttackType.Range:
                    damage = attackPoints * attackPoints / (attackPoints + rangeDefence);
                    break;

                case AttackType.Ability:
                    damage = attackPoints * attackPoints / (attackPoints + abilityDefence);
                    break;

                case AttackType.Ultimate:
                    damage = attackPoints;
                    break;

                default:
                    break;
            }
        }

        if (this && gameObject)
        {
            underAttackFrameCounter = 2;
            hitPoints -= (int)damage;
            if (hitPoints <= 0) Destroy(gameObject);
        }
    }

	public virtual void TakeHeal(int power)
	{
		hitPoints += power;
		if (hitPoints >= maxHitPoints) 
		{
			hitPoints = maxHitPoints;
		}
	}

    public Bounds GetSelectionBounds()
    {
        return selectionBounds;
    }

    public void SetTeamColor()
    {
        TeamColor[] teamColors = GetComponentsInChildren<TeamColor>();
        foreach (TeamColor teamColor in teamColors) teamColor.GetComponent<Renderer>().material.color = player.teamColor;
    }

    private bool TargetInFrontOfWeapon(WorldObject target)
    {
        if (WorkManager.IsObjectFacingTarget(this, target)) return true;
        else return false;
    }

    private bool IsTargetSelf(WorldObject target)
    {
        return this == target;
    }

    protected virtual void AimAtTarget(WorldObject target)
    {
        aiming = true;
        //this behaviour needs to be specified by a specific object
    }

    protected virtual void FireProjectile(WorldObject target, string projectileName, Vector3 spawnPoint, int damage)
    {
        FireProjectile(
            target,
            projectileName,
            spawnPoint,
            Quaternion.LookRotation(target.transform.position - transform.position),
            damage
        );
    }

    protected virtual void FireProjectile(WorldObject target, string projectileName, Vector3 spawnPoint, Quaternion rotation, int damage)
    {
        GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetProjectile(projectileName), spawnPoint, rotation);
        Projectile projectile = gameObject.GetComponentInChildren<Projectile>();
        projectile.Player = this.player;
        projectile.SetRange(this.weaponRange);
        projectile.SetTarget(target);
        projectile.SetDamage(damage);
        projectile.transform.rotation = rotation;
    }

    public virtual void FireProjectile(
        WorldObject target, string projectileName, 
        Vector3 spawnPoint, Quaternion rotation, float range,
        int damage, Status[] statuses)
    {
        GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetProjectile(projectileName), spawnPoint, rotation);
        Projectile projectile = gameObject.GetComponentInChildren<Projectile>();
        projectile.statuses = statuses;
        projectile.Player = this.player;
        projectile.SetRange(range);
        projectile.SetTarget(target);
        projectile.SetDamage(damage);
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
        currentAttackDelayTime = 0.0f;
        //this behaviour needs to be specified by a specific object
    }

    protected virtual void UseWeaponMulti(List<WorldObject> target)
    {
        if (audioElement != null && Time.timeScale > 0) audioElement.Play(useWeaponSound);

        currentWeaponMultiChargeTime = 0.0f;
        currentAttackDelayTime = 0.0f;
        //this behaviour needs to be specified by a specific object
    }
   
    private void RemoveInactiveStatuses()
    {
        var stillActiveStatuses = new List<Status>();

        ActiveStatuses.ForEach(p =>
        {
            if (p && p.isActive)
            {
                stillActiveStatuses.Add(p);
            }
            else
            {
                Destroy(p);
            }
        });

        ActiveStatuses = stillActiveStatuses;
    }

    private void HandleSelectionLight()
    {
        if (selectionLight)
        {
            selectionLight.enabled = player && player.human && currentlySelected;
            /*
            selectionLight.enabled = player &&
                (
                    (player.human && currentlySelected) ||
                    (
                        !player.human &&
                        targetManager &&
                        targetManager.SingleTarget &&
                        targetManager.SingleTarget.ObjectId == ObjectId
                    )
                ); */
        }
    }

    private void HandleTargetIndicator ()
    {
        if (targetLight && chasedFrameCounter == 0)
        {
            targetLight.enabled = false;
        }
    }

    private bool MultiAttackDelayIsOver()
    {
        return currentAttackDelayTime > attackMultiDelay;
    }

    private bool AttackDelayIsOver()
    {
        return currentAttackDelayTime > attackDelay;
    }

    private void HandleAnimation ()
    {
        if (!animator) return;

        animator.SetBool("attacking", IsAttacking());
        animator.SetBool("inBattle", !ReadyToFire());
    }

    public WorldObjectData GetData ()
    {
        return new WorldObjectData(
            name.Contains("(") ? name.Substring(0, name.IndexOf("(")).Trim() : name,
            ObjectId,
            objectName,
            hitPoints,
            isBusy,
            ActiveStatuses.Select(status => status.GetData()).ToList(),
            currentlySelected,
            movingIntoPosition,
            aiming,
            aimRotation,
            attackDelayFrameCounter,
            currentWeaponChargeTime,
            currentWeaponMultiChargeTime,
            currentAttackDelayTime,
            isInvincible,
            stateController ? stateController.GetData() : null,
            underAttackFrameCounter,
            fogOfWarAgent ? fogOfWarAgent.GetData() : null,
            transform.position,
            transform.rotation
        );
    }

    public void SetData (WorldObjectData data)
    {
        Start();

        name = data.type;
        ObjectId = data.objectId;
        objectName = data.objectName;
        hitPoints = data.hitPoints;
        isBusy = data.isBusy;
        ActiveStatuses = data.activeStatuse.Select(status =>
        {
            var statusObject = (GameObject)GameObject.Instantiate(ResourceManager.GetStatus(status.type));
            var createdStatus = statusObject.GetComponent<Status>();
            createdStatus.transform.parent = statusesWrapper.transform;

            createdStatus.SetData(status);

            return createdStatus;
        }).ToList();
        currentlySelected = data.currentlySelected;
        movingIntoPosition = data.movingIntoPosition;
        aiming = data.aiming;
        aimRotation = data.aimRotation;
        attackDelayFrameCounter = data.attackDelayFrameCounter;
        currentWeaponChargeTime = data.currentWeaponChargeTime;
        currentWeaponMultiChargeTime = data.currentWeaponMultiChargeTime;
        currentAttackDelayTime = data.currentAttackDelayTime;
        isInvincible = data.isInvincible;
        if (data.stateController != null)
        {
            stateController.SetData(data.stateController);
            stateController.controlledObject = this;
        }
        underAttackFrameCounter = data.underAttackFrameCounter;

        if (data.fogOfWarAgent != null)
        {
            fogOfWarAgent.SetData(data.fogOfWarAgent);
        }

        UpdateChildRenderers();
        CalculateBounds();
        var humanPlayer = PlayerManager.GetHumanPlayer(FindObjectsOfType<Player>());
        if (humanPlayer)
        {
            targetManager = humanPlayer.GetComponentInChildren<TargetManager>();
        }

        SetPlayer();
    }
}
