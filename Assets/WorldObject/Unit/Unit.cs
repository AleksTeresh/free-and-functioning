using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;
using UnityEngine.AI;
using Statuses;
using Abilities;
using Persistence;

public class Unit : WorldObject
{
    private static readonly float HIT_SPHERE_SCALE = 4;

    protected new UnitStateController stateController;

    private AbilityAgent abilityAgent;

    [HideInInspector] public bool holdingPosition = false;
    // public float moveSpeed, rotateSpeed;
    protected NavMeshAgent agent;

    // protected bool moving, rotating;
    protected LocalUI localUI;
    protected Collider hitSphereCollider;

    [Header("Audio")]
    public AudioClip driveSound;
    public AudioClip moveSound;
    public float driveVolume = 0.5f, moveVolume = 1.0f;

    [Header("Attack")]
    protected WorldObject aimTarget;
    private ParticleSystem takeDamageEffect;

    private GameObject destinationIndicator;

    public override void SetHoverState(GameObject hoverObject)
    {
        base.SetHoverState(hoverObject);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (WorkManager.ObjectIsGround(hoverObject)) hud.SetCursorState(CursorState.Move);
        }
    }

    public virtual void Init(Building creator)
    {
        //specific initialization for a indicatedObject can be specified here
    }

    public void StartMove(Vector3 destination)
    {
        if (!isBusy)
        {
            if (audioElement != null) audioElement.Play(driveSound);

            if (agent)
            {
                agent.SetDestination(destination);
            }

            /*
            var newPath = new NavMeshPath();
            bool result = agent.CalculatePath(destination, newPath);

            if (result)
            {
                agent.SetPath(newPath);
                Debug.Log("Path is successfully computed");
            } else
            {
                agent.SetDestination(destination);
            } */
        }
    }

    public void StopMove()
    {
        if (audioElement != null) audioElement.Stop(driveSound);

        agent.isStopped = true;
        agent.ResetPath();
    }
    /*
    public override void SaveDetails(JsonWriter writer)
    {
        base.SaveDetails(writer);
        SaveManager.WriteVector(writer, "Velocity", agent.velocity);
        SaveManager.WriteVector(writer, "Destination", agent.destination);
    }  */

    public override void TakeDamage(int damage, AttackType attackType)
    {
        base.TakeDamage(damage, attackType);

        if (takeDamageEffect)
        {
            takeDamageEffect.Play();
        }
    }

    public override bool CanAddStatus()
    {
        return true;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }

    public Collider GetHitSphere()
    {
        return hitSphereCollider;
    }
    /*
    protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        base.HandleLoadedProperty(reader, propertyName, readValue);
        switch (propertyName)
        {
            case "Velocity": agent.velocity = LoadManager.LoadVector(reader); break;
            case "Destination": agent.destination = LoadManager.LoadVector(reader); break;
            default: break;
        }
    } */

    public virtual bool IsMajor()
    {
        return false;
    }

    public new UnitStateController GetStateController()
    {
        return stateController;
    }

    public AbilityAgent GetAbilityAgent()
    {
        return abilityAgent;
    }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        takeDamageEffect = GetComponentInChildren<ParticleSystem>();

        stateController = GetComponent<UnitStateController>();
    }

    protected override void Start()
    {
        base.Start();

        // instantiate localUI
        var localUIObject = Instantiate(ResourceManager.GetUIElement("LocalUI"), transform);
        localUI = localUIObject.GetComponent<LocalUI>();

        // instantiate a hit sphere based on selection bounds
        var hitSphereObject = Instantiate(ResourceManager.GetWorldObject("HitSphere"), transform);
        hitSphereObject.transform.localScale = new Vector3(1, 1, 1) * HIT_SPHERE_SCALE;
        this.hitSphereCollider = hitSphereObject.GetComponent<Collider>();

        // instantiate abilityUser
        var abilitiesWrapper = GetComponentInChildren<Abilities.Abilities>();
        var abilitiesMultiWrapper = GetComponentInChildren<AbilitiesMulti>();
        var abilityAgentObject = Instantiate(ResourceManager.GetAbilityAgent(), transform);
        abilityAgent = abilityAgentObject.GetComponent<AbilityAgent>();
        abilityAgent.Init(abilitiesWrapper, abilitiesMultiWrapper);
    }
    protected override void Update()
    {
        base.Update();

        this.isBusy = abilityAgent != null && abilityAgent.IsAnyAbilityPending();

        HandleMove();
        HandleRotation();
        HandleVisualEffects();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (GetNavMeshAgent())
        {
            var navMeshAgent = GetNavMeshAgent();
            Gizmos.color = navMeshAgent.pathPending ? Color.yellow : Color.green;
            Gizmos.DrawSphere(this.GetNavMeshAgent().destination, 3);
        }
    }

    protected override void InitialiseAudio()
    {
        base.InitialiseAudio();
        List<AudioClip> sounds = new List<AudioClip>();
        List<float> volumes = new List<float>();
        if (driveVolume < 0.0f) driveVolume = 0.0f;
        if (driveVolume > 1.0f) driveVolume = 1.0f;
        volumes.Add(driveVolume);
        sounds.Add(driveSound);
        if (moveVolume < 0.0f) moveVolume = 0.0f;
        if (moveVolume > 1.0f) moveVolume = 1.0f;
        sounds.Add(moveSound);
        volumes.Add(moveVolume);
        audioElement.Add(sounds, volumes);
    }

    protected override void AimAtTarget(WorldObject target)
    {
        base.AimAtTarget(target);
        aimRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        aimTarget = target;
    }

    private void HandleMove()
    {
        if (
            (
            agent.velocity.magnitude == 0 &&
            (transform.position - agent.destination).sqrMagnitude <= agent.stoppingDistance * agent.stoppingDistance
            ) ||
            isBusy
        )
        {
            if (audioElement != null) audioElement.Stop(driveSound);

            // moving = false;
            agent.isStopped = true;
            movingIntoPosition = false;
        }
        else
        {
            agent.isStopped = false;
            CalculateBounds();
        }
    }

    private void HandleRotation()
    {
        if (!isBusy && aiming && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && aimTarget)
        {
            aimRotation.x = 0;
            aimRotation.z = 0;
            var nextAimRotation = Quaternion.Slerp(transform.rotation, aimRotation, weaponAimSpeed * Time.deltaTime);
            nextAimRotation.x = 0;
            nextAimRotation.z = 0;
            transform.rotation = nextAimRotation;
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, weaponAimSpeed);
            CalculateBounds();

            //sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
            // Quaternion inverseAimRotation = new Quaternion(-aimRotation.x, -aimRotation.y, -aimRotation.z, -aimRotation.w);

            if (WorkManager.IsObjectFacingTarget(this, aimTarget))
            {
                aiming = false;
            }
        }
    }

    private void HandleVisualEffects()
    {
        if (player && player.human && IsSelected() && (agent.remainingDistance > agent.stoppingDistance || agent.pathPending))
        {
            if (!destinationIndicator)
            {
                destinationIndicator = Instantiate(ResourceManager.GetVfx("DestinationIndicator"), agent.destination, transform.rotation);
            }
            else
            {
                destinationIndicator.transform.position = agent.destination;
            }
        }
        else if (destinationIndicator)
        {
            Destroy(destinationIndicator);
        }
    }

    public new UnitData GetData()
    {
        var baseData = base.GetData();

        return new UnitData(
            baseData,
            stateController.GetData(),
            abilityAgent.GetData(),
            holdingPosition,
            aimTarget ? aimTarget.ObjectId : -1,
            agent.destination
        );
    }

    public void SetData (UnitData data)
    {
        base.SetData(data);
        Start();

        if (data.type == "Level1_Tanker" || data.type == "Level1_Healer" || data.type == "Level1_DamageDealer")
        {
            Debug.Log("Got it");
        }

        aimTarget = data.aimTargetId != -1
            ? Player.GetObjectById(data.aimTargetId)
            : null;
        // agent.SetDestination(data.destination);
        holdingPosition = data.holdingPosition;
        stateController.SetData(data.unitStateController);
        stateController.unit = this;
    }
}
