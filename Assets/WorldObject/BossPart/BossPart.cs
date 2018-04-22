using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Abilities;

public class BossPart : WorldObject {
    private GameObject destinationTarget;
    private int loadedDestinationTargetId = -1;

    [Header("Audio")]
    public AudioClip moveSound;
    public float moveVolume = 1.0f;

    [HideInInspector] public Ability[] abilities;
    [HideInInspector] public Ability[] abilitiesMulti;

    [Header("Attack")]
    protected Quaternion aimRotation;
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

    public Ability FindAbilityByIndex(int abilityIndex)
    {
        if (abilityIndex < abilities.Length)
        {
            return abilities[abilityIndex];
        }

        return null;
    }

    public Ability FindAbilityMultiByIndex(int abilityIndex)
    {
        if (abilityIndex < abilitiesMulti.Length)
        {
            return abilitiesMulti[abilityIndex];
        }

        return null;
    }

    public override bool CanAddStatus()
    {
        return true;
    }

    public bool CanUseAbilitySlot(int slotIdx)
    {
        return AbilityUtils.CanUseAbilitySlot(abilities, abilitiesMulti, slotIdx);
    }

    public Ability GetFirstReadyAbility()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (CanUseAbilitySlot(i))
            {
                return abilities[i];
            }
        }

        return null;
    }

    public Ability GetFirstReadyMultiAbility()
    {
        for (int i = 0; i < abilitiesMulti.Length; i++)
        {
            if (CanUseAbilitySlot(i))
            {
                return abilitiesMulti[i];
            }
        }

        return null;
    }

    public virtual bool IsMajor()
    {
        return true;
    }

    protected override void Awake()
    {
        base.Awake();

        takeDamageEffect = GetComponentInChildren<ParticleSystem>();

        var abilitiesWrapper = GetComponentInChildren<Abilities.Abilities>();
        var abilitiesMultiWrapper = GetComponentInChildren<AbilitiesMulti>();

        if (abilitiesWrapper)
        {
            abilities = abilitiesWrapper.GetComponentsInChildren<Ability>();
        }

        if (abilitiesMultiWrapper)
        {
            abilitiesMulti = abilitiesMultiWrapper.GetComponentsInChildren<Ability>();
        }

        stateController = GetComponent<StateController>();
    }

    protected override void Start()
    {
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
    }

    protected override void Update()
    {
        base.Update();

        this.isBusy = IsAnyAbilityPending();

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

    private bool IsAnyAbilityPending()
    {
        foreach (var ability in abilities)
        {
            if (ability.isPending)
            {
                return true;
            }
        }

        foreach (var ability in abilitiesMulti)
        {
            if (ability.isPending)
            {
                return true;
            }
        }

        return false;
    }
}
