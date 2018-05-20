using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Formation;
using Dialog;
using Events;

public class UserInput : MonoBehaviour
{

    private Player player;
    private DialogManager dialogManager;
    private TargetManager targetManager;
    private FormationManager formationManager;
    private HUD hud;
    private Camera mainCamera;
    private Ground ground;

    // Use this for initialization
    void Start()
    {
        player = transform.root.GetComponent<Player>();
        targetManager = transform.root.GetComponentInChildren<TargetManager>();
        formationManager = transform.root.GetComponentInChildren<FormationManager>();
        dialogManager = transform.root.GetComponentInChildren<DialogManager>();
        hud = transform.root.GetComponentInChildren<HUD>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        ground = FindObjectOfType<Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.human)
        {
            // allow player to control dialog system only when gameplay is blocked,
            // if it is not, the flow of text should be controlled from another place,
            // for example, from SceneDriver
            if (dialogManager && dialogManager.BlockGameplay)
            {
                HandleDialogInput();
            }
            else
            {
                MoveCameraWithMouseScroll();
                MoveCameraWithGamepad();
                RotateCamera();

                MouseActivity();

                AttackModeSelection();

                SwitchEnemy();

                StopUnits();

                SwitchHoldPosition();

                SwitchFormationType();

                HandlePauseMenu();

                RTS.HotkeyUnitSelector.HandleInput(player, hud, mainCamera);
                RTS.HotkeyAbilitySelector.HandleInput(player, targetManager);
                RTS.HotkeyAllyAbilityTargetSelector.HandleInput(player, hud);
            }
        }
    }

    private void MoveCameraWithGamepad()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        float xAxis = 0.0f;
        float yAxis = 0.0f;
        float zAxis = 0.0f;

        if (Gamepad.GetButton("SelectionModifier"))
        {
            yAxis = -1f * Gamepad.GetAxis("Camera Move Z");
        }
        else
        {
            xAxis = Gamepad.GetAxis("Camera Move X");
            zAxis = Gamepad.GetAxis("Camera Move Z");
        }

        movement.x = xAxis;
        movement.y = yAxis;
        movement.z = zAxis;

        MoveCamera(movement);
    }

    private void MoveCameraWithMouseScroll()
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

        MoveCamera(movement);

        if (!mouseScroll)
        {
            hud.SetCursorState(CursorState.Select);
        }
    }

    private void MoveCamera(Vector3 movement)
    {
        if (!Camera.main) return;

        // calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        destination.y = Mathf.Min(
            ResourceManager.MaxCameraHeight,
            Mathf.Max(ResourceManager.MinCameraHeight, destination.y)
        );

        // restrict camera movements to be within the terrain
        if (ground && ground.Terrain && ground.Terrain.terrainData)
        {
            destination.x = Mathf.Min(ground.Terrain.terrainData.size.x, Mathf.Max(0, destination.x));
            destination.z = Mathf.Min(ground.Terrain.terrainData.size.z, Mathf.Max(0, destination.z));
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
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
        if (player.selectedAllyTargettingAbility == null && (Input.GetButtonDown("Attack Mode") || Gamepad.GetButtonDown("Attack Mode")))
        {
            // get all the objects controlled through AI
            var stateControlles = player.GetComponentsInChildren<UnitStateController>();
            EventManager.TriggerEvent("SwitchAttackModeCommand");
            InputToCommandManager.SwitchAttackMode(targetManager, new List<UnitStateController>(stateControlles));
        }
    }

    private void HandleDialogInput()
    {
        if (!dialogManager || !dialogManager.IsActive())
        {
            return;
        }

        if (!dialogManager.GetDialogResponsePanel().IsOpen() && (Input.GetButtonDown("Submit") || Gamepad.GetButtonDown("Ability4")))
        {
            dialogManager.DisplayNextSentence();
        }
    }

    private void SwitchEnemy()
    {
        if (Input.GetButtonDown("NextEnemy") || (Gamepad.GetButton("SelectionModifier") && Gamepad.GetButtonDown("Ability3")))
        {
            var majorVisibleEnemies = UnitManager.GetPlayerVisibleMajorEnemies(player).Cast<WorldObject>();
            var visibleBuildings = UnitManager.GetVisibleEnemyBuildings(player);
            var visibleBossParts = UnitManager.GetVisibleEnemyBossParts(player);

            var enemiesToDisplay = majorVisibleEnemies
                .Concat(visibleBuildings)
                .Concat(visibleBossParts)
                .ToList();

            enemiesToDisplay.Sort((a, b) => a.ObjectId - b.ObjectId);

            if (targetManager)
            {
                int selectionIdx = WorkManager.GetTargetSelectionIndex(targetManager.SingleTarget, enemiesToDisplay);

                EventManager.TriggerEvent("SwitchEnemyCommand");
                InputToCommandManager.SwitchEnemy(targetManager, enemiesToDisplay, selectionIdx);
            }
        }
    }

    private void StopUnits()
    {
        if (Input.GetButtonDown("StopUnits") || (Gamepad.GetButton("SelectionModifier") && Gamepad.GetButtonDown("Ability4")))
        {
            EventManager.TriggerEvent("StopCommand");
            InputToCommandManager.StopUnits(player, targetManager);
        }
    }

    private void SwitchHoldPosition()
    {
        if (Input.GetButtonDown("HoldPosition") || (Gamepad.GetButton("SelectionModifier") && Gamepad.GetButtonDown("Ability2")))
        {
            EventManager.TriggerEvent("SwitchHoldPositionCommand");
            InputToCommandManager.SwitchHoldPosition(player);
        }
    }

    private void SwitchFormationType()
    {
        if (Input.GetButtonDown("SwitchFormation") || (Gamepad.GetButton("SelectionModifier") && Gamepad.GetButtonDown("Ability1")))
        {
            EventManager.TriggerEvent("SwitchFormationCommand");
            InputToCommandManager.SwitchFormationType(formationManager);
        }
    }

    private void MouseActivity()
    {
        if (Input.GetButtonDown("Action 1"))
        {
            // lock cursor back if it was unlocked from editor
            if (player.lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            LeftMouseClick();
        }
        else if (Input.GetMouseButtonDown(1) || Gamepad.GetButtonDown("Action 1"))
        {
            RightMouseClick();
        }

        MouseHover();
    }

    private void LeftMouseClick()
    {
        if (hud.CursorInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (!WorkManager.ObjectIsGround(hitObject))
                {
                    if (!hitObject.transform.parent) return;

                    Unit unitToSelect = hitObject.transform.parent.GetComponent<Unit>();
                    if (unitToSelect)
                    {
                        if (player.selectedAllyTargettingAbility != null)
                        {
                            AbilityUtils.ApplyAllyAbilityToTarget(unitToSelect, player);
                        }
                        else if (Input.GetButton("SelectionModifier") || Gamepad.GetButton("SelectionModifier"))
                        {
                            UnitSelectionManager.HandleUnitSelectionWithModifierPress(unitToSelect, player, hud);
                        }
                        else
                        {
                            UnitSelectionManager.HandleUnitSelection(unitToSelect, player, mainCamera, hud);
                        }
                    }
                }
                else if (player.SelectedObject)
                {
                    player.SelectedObject.SetSelection(false, hud.GetPlayingArea());
                    player.SelectedObject = null;
                    player.selectedObjects
                        .Where(p => p != null)
                        .ToList()
                        .ForEach(p => p.SetSelection(false, hud.GetPlayingArea()));
                    player.selectedObjects = new List<WorldObject>();
                }
            }
        }
    }

    private void RightMouseClick()
    {
        if (hud.CursorInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject) MouseClick(hitObject, hitPoint);
            }
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
            if (!hitObject.transform.parent) return;

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
                        var handlerStateController = objectHandler.GetStateController();
                        //start attack if object is not owned by the same player and this object can attack, else select
                        if (handlerStateController && player.username != owner.username && objectHandler.CanAttack())
                        {
                            InputToCommandManager.ToChaseState(targetManager, handlerStateController, worldObject);
                        }
                        else
                        {
                            IssueMoveOrderToSelectedUnits(hitObject, hitPoint);
                            // ChangeSelection(objectHandler, worldObject);
                        }
                    }
                    // else ChangeSelection(objectHandler, worldObject);
                }
                else if (objectHandler.CanAttack())
                {
                    if (!hitObject.transform.parent) return;

                    Unit unit = hitObject.transform.parent.GetComponent<Unit>();
                    Building building = hitObject.transform.parent.GetComponent<Building>();
                    BossPart bossPart = hitObject.transform.parent.GetComponent<BossPart>();

                    var handlerStateController = objectHandler.GetStateController();

                    if ((unit || building || bossPart) && handlerStateController) InputToCommandManager.ToChaseState(targetManager, handlerStateController, worldObject);
                }
                // else ChangeSelection(objectHandler, worldObject);
            }
        }
    }

    private void UnitMouseClick(Unit objectHandler, GameObject hitObject, Vector3 hitPoint)
    {
        Player owner = objectHandler.GetComponentInParent<Player>();
        // Unit unit = hitObject.transform.parent.GetComponent<Unit>();
        Player hitObjectOwner = hitObject.GetComponentInParent<Player>();
        //only handle input if owned by a human player and currently selected

        //issue move order to one or more units
        if (
            owner.username == player.username &&
            (!hitObjectOwner || hitObjectOwner.name == player.username) &&
            player &&
            player.human &&
            objectHandler.IsSelected()
        )
        {
            IssueMoveOrderToUnit(objectHandler, hitObject, hitPoint, Vector3.zero);

            if (player.selectedObjects.Count > 0)
            {
                IssueMoveOrderToSelectedUnits(hitObject, hitPoint);
            }
        }
    }


    private void IssueMoveOrderToUnit(Unit objectHandler, GameObject hitObject, Vector3 hitPoint, Vector3 formationOffset)
    {
        var handlerStateController = objectHandler.GetStateController();
        if (handlerStateController && hitPoint != ResourceManager.InvalidPosition)
        {
            Vector3 idealDestination = hitPoint + formationOffset;
            if (!WorkManager.ObjectIsGround(hitObject))
            {
                Vector3? onNavMeshDest = WorkManager.GetClosestPointOnNavMesh(hitPoint, "Walkable", 7);

                if (onNavMeshDest.HasValue)
                {
                    idealDestination = onNavMeshDest.Value;
                }
            }

            Vector3 actualDestination = hitPoint;
            Vector3 destDifference = hitPoint - idealDestination;

            for (int i = 0; i < 5; i++)
            {
                var potentialDest = WorkManager.GetClosestPointOnNavMesh(idealDestination + (destDifference / 4 * i), "Walkable", 2);

                if (potentialDest.HasValue)
                {
                    actualDestination = potentialDest.Value;
                    break;
                }
            }

            EventManager.TriggerEvent("RelocateUnit");
            InputToCommandManager.ToBusyState(targetManager, handlerStateController, actualDestination);
        }
    }

    private void IssueMoveOrderToSelectedUnits(GameObject hitObject, Vector3 hitPoint)
    {
        for (int i = 0; i < player.selectedObjects.Count; i++)
        {
            Unit unit = (Unit)player.selectedObjects[i];

            var formationOffset = Vector3.zero;
            switch (formationManager.CurrentFormationType)
            {
                case FormationType.Auto:
                    formationOffset = FormationUtils.CalculateAutoFormationOffset(player, unit, hitPoint);
                    break;

                case FormationType.Manual:
                    formationOffset = FormationUtils.CalculateManualFormationOffset(player, unit);
                    break;

                default:
                    break;
            }

            IssueMoveOrderToUnit(unit, hitObject, hitPoint, formationOffset);
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

        bool groundWasHit = false;
        GameObject ground = null;

        // first try without hitSphere enabled
        if (Physics.Raycast(ray, out hit))
        {
            var relatedMesh = hit.collider.gameObject.GetComponent<MeshRenderer>();

            if (WorkManager.ObjectIsGround(hit.collider.gameObject))
            {
                groundWasHit = true;
                ground = hit.collider.gameObject;
            }
            else if (relatedMesh && relatedMesh.enabled)
            {
                return hit.collider.gameObject;
            }
            // return null;
        }

        var nearbyUnits = WorkManager.FindNearbyUnits(hit.point, 7);
        nearbyUnits = nearbyUnits.Where(p => p && p.GetFogOfWarAgent() && p.GetFogOfWarAgent().IsObserved()).ToList();

        nearbyUnits.ForEach(p =>
        {
            if (p.GetHitSphere())
            {
                ((Unit)p).GetHitSphere().enabled = true;
            }
        });

        // try with hit sphere enabled
        if (Physics.Raycast(ray, out hit))
        {
            var relatedMesh = hit.collider.gameObject;

            if (relatedMesh.GetComponent<HitSphere>())
            {
                nearbyUnits.ForEach(p =>
                {
                    if (p.GetHitSphere())
                    {
                        p.GetHitSphere().enabled = false;
                    }
                });

                return hit.collider.gameObject;
            }
        }

        nearbyUnits.ForEach(p =>
        {
            if (p.GetHitSphere())
            {
                p.GetHitSphere().enabled = false;
            }
        });

        // finally check if at least ground was hit
        if (groundWasHit)
        {
            return ground;
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

    private void HandlePauseMenu()
    {
        if (
            player.selectedAllyTargettingAbility == null &&
            player.selectedAlliesTargettingAbility == null &&
           (Input.GetButtonDown("Cancel") || Gamepad.GetButtonDown("Start")) &&
            !ResourceManager.MenuOpen
        )
        {
            hud.TogglePauseMenu();
        }
    }

    private void MouseHover()
    {
        if (hud.CursorInBounds())
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
                        BossPart bossPart = hoverObject.transform.parent.GetComponent<BossPart>();

                        if (owner.username == player.username && (unit || building || bossPart)) hud.SetCursorState(CursorState.Select);
                    }
                }
            }
        }
    }
}
