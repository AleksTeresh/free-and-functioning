using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Unit : WorldObject {

    public float moveSpeed, rotateSpeed;

    protected bool moving, rotating;

    private Vector3 destination;
    private Quaternion targetRotation;
    private GameObject destinationTarget;

    public override void SetHoverState(GameObject hoverObject)
    {
        base.SetHoverState(hoverObject);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (hoverObject.name == "Ground") player.hud.SetCursorState(CursorState.Move);
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
            if (hitObject.name == "Ground" && hitPoint != ResourceManager.InvalidPosition)
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
        this.destinationTarget = null;
        this.destination = destination;
        targetRotation = Quaternion.LookRotation(destination - transform.position);
        rotating = true;
        moving = false;
    }

    public void StartMove(Vector3 destination, GameObject destinationTarget)
    {
        StartMove(destination);
        this.destinationTarget = destinationTarget;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (rotating) TurnToTarget();
        else if (moving) MakeMove();
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        if (currentlySelected) DrawSelection();
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

    private void TurnToTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);
        //sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
        Quaternion inverseTargetRotation = new Quaternion(-targetRotation.x, -targetRotation.y, -targetRotation.z, -targetRotation.w);
        if (transform.rotation == targetRotation || transform.rotation == inverseTargetRotation)
        {
            rotating = false;
            moving = true;
            if (destinationTarget) CalculateTargetDestination();
        }

        CalculateBounds();
    }

    private void MakeMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
        if (transform.position == destination)
        {
            moving = false;
            movingIntoPosition = false;
        }

        CalculateBounds();
    }

    private void CalculateTargetDestination()
    {
        //calculate number of unit vectors from unit centre to unit edge of bounds
        Vector3 originalExtents = selectionBounds.extents;
        Vector3 normalExtents = originalExtents;
        normalExtents.Normalize();
        float numberOfExtents = originalExtents.x / normalExtents.x;
        int unitShift = Mathf.FloorToInt(numberOfExtents);

        //calculate number of unit vectors from target centre to target edge of bounds
        WorldObject worldObject = destinationTarget.GetComponent<WorldObject>();
        if (worldObject) originalExtents = worldObject.GetSelectionBounds().extents;
        else originalExtents = new Vector3(0.0f, 0.0f, 0.0f);
        normalExtents = originalExtents;
        normalExtents.Normalize();
        numberOfExtents = originalExtents.x / normalExtents.x;
        int targetShift = Mathf.FloorToInt(numberOfExtents);

        //calculate number of unit vectors between unit centre and destination centre with bounds just touching
        int shiftAmount = targetShift + unitShift;

        //calculate direction unit needs to travel to reach destination in straight line and normalize to unit vector
        Vector3 origin = transform.position;
        Vector3 direction = new Vector3(destination.x - origin.x, 0.0f, destination.z - origin.z);
        direction.Normalize();

        //destination = center of destination - number of unit vectors calculated above
        //this should give us a destination where the unit will not quite collide with the target
        //giving the illusion of moving to the edge of the target and then stopping
        for (int i = 0; i < shiftAmount; i++) destination -= direction;
        destination.y = destinationTarget.transform.position.y;
    }
}
