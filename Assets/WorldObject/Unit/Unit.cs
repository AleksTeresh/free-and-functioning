using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;
using UnityEngine.AI;

public class Unit : WorldObject {

    // public float moveSpeed, rotateSpeed;
    private NavMeshAgent agent;

    // protected bool moving, rotating;

    // private Vector3 destination;
    private Quaternion targetRotation;
    private GameObject destinationTarget;

    private int loadedDestinationTargetId = -1;

    // audio 
    public AudioClip driveSound, moveSound;
    public float driveVolume = 0.5f, moveVolume = 1.0f;

	private ParticleSystem takeDamageEffect;


    public override void SetHoverState(GameObject hoverObject)
    {
        base.SetHoverState(hoverObject);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (WorkManager.ObjectIsGround(hoverObject)) player.hud.SetCursorState(CursorState.Move);
        }
    }

    public virtual void Init(Building creator)
    {
        //specific initialization for a unit can be specified here
    }

    public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        base.MouseClick(hitObject, hitPoint, controller);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (WorkManager.ObjectIsGround(hitObject) && hitPoint != ResourceManager.InvalidPosition)
            {
                float x = hitPoint.x;
                //makes sure that the unit stays on top of the surface it is on
                float y = hitPoint.y + player.SelectedObject.transform.position.y;
                float z = hitPoint.z;
                Vector3 destination = new Vector3(x, y, z);
                StartMove(destination);
            }
        }
    }

    public void StartMove(Vector3 destination)
    {
        if (audioElement != null) audioElement.Play(driveSound);

        this.destinationTarget = null;
        agent.SetDestination(destination);
//        targetRotation = Quaternion.LookRotation(destination - transform.position);
        // rotating = true;
        // moving = false;
    }

    public void StartMove(Vector3 destination, GameObject destinationTarget)
    {
        StartMove(destination);
        this.destinationTarget = destinationTarget;
    }

    public override void SaveDetails(JsonWriter writer)
    {
        base.SaveDetails(writer);
        SaveManager.WriteVector(writer, "Velocity", agent.velocity);
        SaveManager.WriteVector(writer, "Destination", agent.destination);
        SaveManager.WriteQuaternion(writer, "TargetRotation", targetRotation);
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

    protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        base.HandleLoadedProperty(reader, propertyName, readValue);
        switch (propertyName)
        {
            case "Velocity": agent.velocity = LoadManager.LoadVector(reader); break;
            case "Destination": agent.destination = LoadManager.LoadVector(reader); break;
            case "TargetRotation": targetRotation = LoadManager.LoadQuaternion(reader); break;
            case "DestinationTargetId": loadedDestinationTargetId = (int)(System.Int64)readValue; break;
            default: break;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
		takeDamageEffect = GetComponentInChildren<ParticleSystem> ();
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
        // if (rotating) TurnToTarget();
        // else if (moving) MakeMove();
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
