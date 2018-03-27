﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class UserInput : MonoBehaviour {

    private Player player;

    // Use this for initialization
    void Start () {
        player = transform.root.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        if (player.human)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !ResourceManager.MenuOpen)
            {
                OpenPauseMenu();
            }
            
            MoveCamera();
            RotateCamera();

            MouseActivity();

			RTS.HotkeyUnitSelector.HandleInput ();
        }
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0.0f;
        GetComponentInChildren<PauseMenu>().enabled = true;
        GetComponent<UserInput>().enabled = false;
        Cursor.visible = true;
        ResourceManager.MenuOpen = true;
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        bool mouseScroll = false;

        //horizontal camera movement
        if (xpos >= 0 && xpos < ResourceManager.ScrollWidth)
        {
            movement.x -= ResourceManager.ScrollSpeed;
            player.hud.SetCursorState(CursorState.PanLeft);
            mouseScroll = true;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)
        {
            movement.x += ResourceManager.ScrollSpeed;
            player.hud.SetCursorState(CursorState.PanRight);
            mouseScroll = true;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < ResourceManager.ScrollWidth)
        {
            movement.z -= ResourceManager.ScrollSpeed;
            player.hud.SetCursorState(CursorState.PanDown);
            mouseScroll = true;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth)
        {
            movement.z += ResourceManager.ScrollSpeed;
            player.hud.SetCursorState(CursorState.PanUp);
            mouseScroll = true;
        }

        // make sure movement is in the direction the camera is pointing
        // but ignore the vertical tilt of the camera to get sensible scrolling
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        // away from ground movement
        movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        // calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        if (destination.y > ResourceManager.MaxCameraHeight)
        {
            destination.y = ResourceManager.MaxCameraHeight;
        }
        else if (destination.y < ResourceManager.MinCameraHeight)
        {
            destination.y = ResourceManager.MinCameraHeight;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }

        if (!mouseScroll)
        {
            player.hud.SetCursorState(CursorState.Select);
        }
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
        }
    }

    private void MouseActivity()
    {
        if (Input.GetMouseButtonDown(0)) {
            LeftMouseClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightMouseClick();
        }

        MouseHover();
    }

    private void LeftMouseClick()
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject) MouseClick(hitObject, hitPoint, player);
                else if (!WorkManager.ObjectIsGround(hitObject))
                {
                    WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
                    if (worldObject)
                    {
                        //we already know the player has no selected object
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true, player.hud.GetPlayingArea());
                    }
                }
            }
        }
    }

    private void RightMouseClick()
    {
        if (player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
            player.SelectedObject = null;
        }
    }

    private void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        WorldObject objectHandler = controller.SelectedObject;

        WorldObjectMouseClick(objectHandler, hitObject, hitPoint, controller);

        Unit unit = objectHandler.GetComponent<Unit>();
        if (unit != null)
        {
            UnitMouseClick(unit, hitObject, hitPoint, controller);
        }
    }
		
    private void WorldObjectMouseClick(WorldObject objectHandler, GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle input if currently selected
        if (objectHandler.IsSelected() && !WorkManager.ObjectIsGround(hitObject))
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
                        StateController handlerStateController = objectHandler.GetStateController();
                        //start attack if object is not owned by the same player and this object can attack, else select
                        if (handlerStateController && player.username != owner.username && objectHandler.CanAttack())
                        {
                            InputToCommandManager.ToChaseState(handlerStateController, worldObject);
                        }
                        else
                        {
                            ChangeSelection(objectHandler, worldObject, controller);
                        }
                    }
                    else ChangeSelection(objectHandler, worldObject, controller);
                }
                else ChangeSelection(objectHandler, worldObject, controller);
            }
        }
    }

    private void UnitMouseClick (Unit objectHandler, GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle input if owned by a human player and currently selected
        if (player && player.human && objectHandler.IsSelected())
        {
            StateController handlerStateController = objectHandler.GetStateController();
            if (handlerStateController && WorkManager.ObjectIsGround(hitObject) && hitPoint != ResourceManager.InvalidPosition)
            {
                float x = hitPoint.x;
                //makes sure that the unit stays on top of the surface it is on
                float y = hitPoint.y + player.SelectedObject.transform.position.y;
                float z = hitPoint.z;
                Vector3 destination = new Vector3(x, y, z);

                InputToCommandManager.ToBusyState(handlerStateController, destination);
            }
        }
    }
/*
    private void BuildingMouseClick (Building objectHandler, GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle iput if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (WorkManager.ObjectIsGround(hitObject))
            {
                if ((player.hud.GetCursorState() == CursorState.RallyPoint || player.hud.GetPreviousCursorState() == CursorState.RallyPoint) && hitPoint != ResourceManager.InvalidPosition)
                {
                    SetRallyPoint(hitPoint);
                }
            }
        }
    }
    */
    private void ChangeSelection(WorldObject unselectedObject, WorldObject selectedObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        unselectedObject.SetSelection(false, player.hud.GetPlayingArea());
        if (controller.SelectedObject) controller.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
        controller.SelectedObject = selectedObject;
        selectedObject.SetSelection(true, controller.hud.GetPlayingArea());
    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
        return null;
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return ResourceManager.InvalidPosition;
    }

    private void MouseHover()
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hoverObject = FindHitObject();
            if (hoverObject)
            {
                if (player.SelectedObject) player.SelectedObject.SetHoverState(hoverObject);
                else if (!WorkManager.ObjectIsGround(hoverObject))
                {
                    Player owner = hoverObject.transform.root.GetComponent<Player>();
                    if (owner)
                    {
                        Unit unit = hoverObject.transform.parent.GetComponent<Unit>();
                        Building building = hoverObject.transform.parent.GetComponent<Building>();
                        if (owner.username == player.username && (unit || building)) player.hud.SetCursorState(CursorState.Select);
                    }
                }
            }
        }
    }
}
