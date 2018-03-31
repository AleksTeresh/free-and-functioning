using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;
using UnityEngine.AI;
using Statuses;
using Abilities;

public class Unit : WorldObject {

    // audio 
    public AudioClip driveSound, moveSound;
    public float driveVolume = 0.5f, moveVolume = 1.0f;
	public Status[] activeStatuses;
	public Ability[] abilities;

	protected Quaternion aimRotation;
	private ParticleSystem takeDamageEffect;

	// public float moveSpeed, rotateSpeed;
	protected NavMeshAgent agent;

	// protected bool moving, rotating;

	private GameObject destinationTarget;
	private int loadedDestinationTargetId = -1;


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
        //specific initialization for a unit can be specified here
    }

    public void StartMove(Vector3 destination)
    {
        if (audioElement != null) audioElement.Play(driveSound);

        this.destinationTarget = null;
        agent.SetDestination(destination);

        // StopAttack();
    }

    public void StartMove(Vector3 destination, GameObject destinationTarget)
    {
        StartMove(destination);
        this.destinationTarget = destinationTarget;
    }

    public void StopMove()
    {
        if (audioElement != null) audioElement.Stop(driveSound);

        this.destinationTarget = null;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public override void SaveDetails(JsonWriter writer)
    {
        base.SaveDetails(writer);
        SaveManager.WriteVector(writer, "Velocity", agent.velocity);
        SaveManager.WriteVector(writer, "Destination", agent.destination);
        if (destinationTarget)
        {
            WorldObject destinationObject = destinationTarget.GetComponent<WorldObject>();
            if (destinationObject) SaveManager.WriteInt(writer, "DestinationTargetId", destinationObject.ObjectId);
        }
    }

	public override void TakeDamage (int damage)
	{
		base.TakeDamage (damage);

		takeDamageEffect.Play();
	}

	public Ability FindAbilityByIndex(int abilityIndex) {
		if (abilityIndex < abilities.Length) {
			return abilities [abilityIndex];
		}

		return null;
	}

    protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        base.HandleLoadedProperty(reader, propertyName, readValue);
        switch (propertyName)
        {
            case "Velocity": agent.velocity = LoadManager.LoadVector(reader); break;
            case "Destination": agent.destination = LoadManager.LoadVector(reader); break;
            case "DestinationTargetId": loadedDestinationTargetId = (int)(System.Int64)readValue; break;
            default: break;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
		takeDamageEffect = GetComponentInChildren<ParticleSystem> ();
//		abilities = GetComponents<Ability> ();
    }

    protected override void Start()
    {
        base.Start();

        if (player && loadedSavedValues && loadedDestinationTargetId >= 0)
        {
            destinationTarget = player.GetObjectForId(loadedDestinationTargetId).gameObject;
        }
    }

    protected override void Update()
    {
        base.Update();

        HandleMove();

		if (aiming && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, weaponAimSpeed);
			CalculateBounds();
			//sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
			Quaternion inverseAimRotation = new Quaternion(-aimRotation.x, -aimRotation.y, -aimRotation.z, -aimRotation.w);
			if (transform.rotation == aimRotation || transform.rotation == inverseAimRotation)
			{
				aiming = false;
			}
		}
        // else if (moving) MakeMove();

		for (int i = 0; i < abilities.Length; i++) {
			abilities [i].Update ();
		}
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (currentlySelected) DrawSelection();
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

    private void HandleMove()
    {
        if (agent.velocity.magnitude == 0 && agent.remainingDistance <= agent.stoppingDistance)
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
}
