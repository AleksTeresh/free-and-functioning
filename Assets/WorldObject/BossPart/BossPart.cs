using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Abilities;

public class BossPart : WorldObject {
    private AbilityAgent abilityAgent;

    private GameObject destinationTarget;
    private int loadedDestinationTargetId = -1;

    [Header("Audio")]
    public AudioClip moveSound;
    public float moveVolume = 1.0f;

    [Header("Attack")]
    protected WorldObject aimTarget;
    private ParticleSystem takeDamageEffect;

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

    public virtual bool IsMajor()
    {
        return true;
    }

    public AbilityAgent GetAbilityAgent ()
    {
        return abilityAgent;
    }

    protected override void AwakeObj()
    {
        base.AwakeObj();

        takeDamageEffect = GetComponentInChildren<ParticleSystem>();

        stateController = GetComponent<StateController>();
    }

    protected override void Start()
    {
        AwakeObj();

        underAttackFrameCounter = 0;

        SetPlayer();
        if (player)
        {
            hud = player.GetComponentInChildren<HUD>();
            playingArea = hud.GetPlayingArea();

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

        if (player && loadedSavedValues && loadedDestinationTargetId >= 0)
        {
            destinationTarget = player.GetObjectForId(loadedDestinationTargetId).gameObject;
        }

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

        HandleRotation();
    }

    protected override void InitialiseAudio()
    {
        base.InitialiseAudio();
        List<AudioClip> sounds = new List<AudioClip>();
        List<float> volumes = new List<float>();

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

    private void HandleRotation()
    {
        if (!isBusy && aiming && aimTarget)
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
}
