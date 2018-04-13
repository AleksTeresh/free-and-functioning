using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class UserInput : MonoBehaviour {

    private Player player;
    private TargetManager targetManager;
    private HUD hud;
    private Camera mainCamera;

    // Use this for initialization
    void Start () {
        player = transform.root.GetComponent<Player>();
        targetManager = transform.root.GetComponentInChildren<TargetManager>();
        hud = transform.root.GetComponentInChildren<HUD>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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

            AttackModeSelection();

			RTS.HotkeyUnitSelector.HandleInput (player, hud, mainCamera);
			RTS.HotkeyAbilitySelector.HandleInput(player, targetManager);
			RTS.HotkeyAllyAbilityTargetSelector.HandleInput (player, hud);
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
        float xpos = hud.cursorPosition.x;
        float ypos = hud.cursorPosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        bool mouseScroll = false;

        //horizontal camera movement
        if (xpos >= 0 && xpos < ResourceManager.ScrollWidth)
        {
            movement.x -= ResourceManager.ScrollSpeed;
            hud.SetCursorState(CursorState.PanLeft);
            mouseScroll = true;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)
        {
            movement.x += ResourceManager.ScrollSpeed;
            hud.SetCursorState(CursorState.PanRight);
            mouseScroll = true;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < ResourceManager.ScrollWidth)
        {
            movement.z -= ResourceManager.ScrollSpeed;
            hud.SetCursorState(CursorState.PanDown);
            mouseScroll = true;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth)
        {
            movement.z += ResourceManager.ScrollSpeed;
            hud.SetCursorState(CursorState.PanUp);
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
            hud.SetCursorState(CursorState.Select);
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

    private void AttackModeSelection()
    {
		if (Input.GetButtonDown("Attack Mode") && player.selectedAllyTargettingAbility == null)
        {
            // get all the objects controlled through AI
            var stateControlles = player.GetComponentsInChildren<StateController>();
            InputToCommandManager.SwitchAttackMode(targetManager, new List<StateController>(stateControlles));
        }
    }

    private void MouseActivity()
    {
        if (Input.GetButtonDown("Action 1")) {
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
        if (hud.MouseInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject) MouseClick(hitObject, hitPoint);
                else if (!WorkManager.ObjectIsGround(hitObject))
                {
                    WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
                    if (worldObject)
                    {
                        //we already know the player has no selected object
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true, hud.GetPlayingArea());
                    }
                }
            }
        }
    }

    private void RightMouseClick()
    {
        if (hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false, hud.GetPlayingArea());
            player.SelectedObject = null;
        }
    }

    private void MouseClick(GameObject hitObject, Vector3 hitPoint)
    {
        WorldObject objectHandler = player.SelectedObject;

        WorldObjectMouseClick(objectHandler, hitObject, hitPoint);

        Unit unit = objectHandler.GetComponent<Unit>();
        if (unit != null)
        {
            UnitMouseClick(unit, hitObject, hitPoint);
        }
    }
		
    private void WorldObjectMouseClick(WorldObject objectHandler, GameObject hitObject, Vector3 hitPoint)
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

                    // get owner of the object handler
                    Player objectHandlerOwner = objectHandler.GetComponentInParent<Player>();
                    if (objectHandlerOwner && player && objectHandlerOwner.username == player.username && player.human)
                    { //this object is controlled by a human player
                        StateController handlerStateController = objectHandler.GetStateController();
                        //start attack if object is not owned by the same player and this object can attack, else select
                        if (handlerStateController && player.username != owner.username && objectHandler.CanAttack())
                        {
                            InputToCommandManager.ToChaseState(targetManager, handlerStateController, worldObject);
                        }
                        else
                        {
                            ChangeSelection(objectHandler, worldObject);
                        }
                    }
                    else ChangeSelection(objectHandler, worldObject);
                }
                else if (objectHandler.CanAttack())
                {
                    Unit unit = hitObject.transform.parent.GetComponent<Unit>();
                    Building building = hitObject.transform.parent.GetComponent<Building>();
                    StateController handlerStateController = objectHandler.GetStateController();

                    if ((unit || building) && handlerStateController) InputToCommandManager.ToChaseState(targetManager, handlerStateController, worldObject);
                }
                else ChangeSelection(objectHandler, worldObject);
            }
        }
    }

    private void UnitMouseClick (Unit objectHandler, GameObject hitObject, Vector3 hitPoint)
    {
        Player owner = objectHandler.GetComponentInParent<Player>();
        //only handle input if owned by a human player and currently selected

		//issue move order to one or more units

        if (owner.username == player.username && player && player.human && objectHandler.IsSelected())
        {
			IssueMoveOrderToUnit (objectHandler, hitObject, hitPoint);

			if (player.selectedObjects.Count > 0)
			{
				IssueMoveOrderToSelectedUnits (hitObject, hitPoint);
			}
        }
    }

	private void IssueMoveOrderToUnit(Unit objectHandler, GameObject hitObject, Vector3 hitPoint, float formationOffset = 0) 
	{
		StateController handlerStateController = objectHandler.GetStateController();
		if (handlerStateController && WorkManager.ObjectIsGround(hitObject) && hitPoint != ResourceManager.InvalidPosition)
		{
			float x = hitPoint.x + formationOffset;
			float y = hitPoint.y;
			float z = hitPoint.z;
			Vector3 destination = new Vector3(x, y, z);

			InputToCommandManager.ToBusyState(targetManager, handlerStateController, destination);
		}
	}

	private void IssueMoveOrderToSelectedUnits(GameObject hitObject, Vector3 hitPoint)
	{
		float formationOffset = 5.0f;

		for (int i = 0; i < player.selectedObjects.Count; i++) 
		{
			Unit unit = (Unit) player.selectedObjects [i];

			IssueMoveOrderToUnit (unit, hitObject, hitPoint, (i + 1) * formationOffset);
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
    private void ChangeSelection(WorldObject unselectedObject, WorldObject selectedObject)
    {
        //this should be called by the following line, but there is an outside chance it will not
        unselectedObject.SetSelection(false, hud.GetPlayingArea());
        if (player.SelectedObject) player.SelectedObject.SetSelection(false, hud.GetPlayingArea());
        player.SelectedObject = selectedObject;
        selectedObject.SetSelection(true, hud.GetPlayingArea());
    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(hud.cursorPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var relatedMesh = hit.collider.gameObject.GetComponent<MeshRenderer>();

            if (WorkManager.ObjectIsGround(hit.collider.gameObject) || (relatedMesh && relatedMesh.enabled))
            {
                return hit.collider.gameObject;
            }

            return null;
        }
 
        return null;
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(hud.cursorPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return ResourceManager.InvalidPosition;
    }

    private void MouseHover()
    {
        if (hud.MouseInBounds())
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
                        if (owner.username == player.username && (unit || building)) hud.SetCursorState(CursorState.Select);
                    }
                }
            }
        }
    }
}
