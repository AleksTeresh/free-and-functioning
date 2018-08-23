using System.Collections.Generic;
using UnityEngine;
using RTS;
using Abilities;
using Persistence;
using System.Linq;
using RTS.Constants;

public class Player : MonoBehaviour {

    public WorldObject SelectedObject { get; set; }
	public List<WorldObject> selectedObjects;
    public Color teamColor;

    public string username;
    public bool human;

    public Ability selectedAllyTargettingAbility;
    public Ability selectedAlliesTargettingAbility;

	public string[] hotkeyToUnitNameMapping;
	public UnitMapping unitMapping;

    public bool lockCursor = true;

    private List<Unit> units;
    private List<Building> buildings;

    private Units unitsWrapper;
    private Buildings buildingsWrapper;
    private HUD hud;
    private FogOfWar fogOfWar;
    private Camera mainCamera;

	void Awake()
	{
		unitMapping = new UnitMapping (hotkeyToUnitNameMapping);
	}
		
    // Use this for initialization
    public void Start () {
        hud = GetComponentInChildren<HUD>();
        unitsWrapper = GetComponentInChildren<Units>();
        buildingsWrapper = GetComponentInChildren<Buildings>();
        units = new List<Unit>(GetComponentsInChildren<Unit>());
        buildings = new List<Building>(GetComponentsInChildren<Building>());

        mainCamera = Camera.main;


        if (human)
        {
            // init for of war
            fogOfWar = FindObjectOfType<FogOfWar>();

            if (fogOfWar)
            {
                fogOfWar.SetRevealers(new List<WorldObject>(GetComponentsInChildren<WorldObject>()));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        units = new List<Unit>(GetComponentsInChildren<Unit>());
        buildings = new List<Building>(GetComponentsInChildren<Building>());

        if (!SelectedObject)
        {
            selectedAllyTargettingAbility = null;
            selectedAlliesTargettingAbility = null;
        }
    }

    public bool IsDead()
    {
        if (buildings != null && buildings.Count > 0) return false;
        if (units != null && units.Count > 0) return false;
        return true;
    }

    public List<Unit> GetUnits()
    {
        return units;
    }

	public Unit FindUnitByIdx(int idx)
	{
		if (idx < units.Count) {
			return units [idx];
		}
			
		return null;
	}

    public List<Building> GetBuildings()
    {
        return buildings;
    }

    public Unit AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator, int siblingIdx = -1)
    {
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = unitsWrapper.transform;
        newUnit.transform.SetSiblingIndex(siblingIdx >= 0 ? siblingIdx : units.Count);
        Unit unitObject = newUnit.GetComponent<Unit>();

        if (unitObject)
        {
            unitObject.Init(creator);
            unitObject.ObjectId = ResourceManager.GetNewObjectId();
            if (spawnPoint != rallyPoint)
            {
                unitObject.StartMove(rallyPoint);

                string stateName = "";
                switch (unitObject.GetStateController().currentState.name)
                {
                    case AIStates.IDLE_IDLER:
                        stateName = AIStates.BUSY_IDLER;
                        break;

                    case AIStates.IDLE_VULNERABLE:
                        stateName = AIStates.BUSY_VULNERABLE;
                        break;

                    case AIStates.IDLE_KITER:
                        stateName = AIStates.BUSY_KITER;
                        break;

                    case AIStates.IDLE_ASSASSIN:
                        stateName = AIStates.BUSY_ASSASSIN;
                        break;

                    case AIStates.IDLE_CCENEMY:
                        stateName = AIStates.BUSY_CCENEMY;
                        break;

                    case AIStates.IDLE_DBENEMY:
                        stateName = AIStates.BUSY_DBENEMY;
                        break;

                    case AIStates.IDLE_DDENEMY:
                        stateName = AIStates.BUSY_DDENEMY;
                        break;

                    case AIStates.PATROL_CHASER:
                        stateName = AIStates.BUSY_CHASER;
                        break;
                }

                if (stateName != "")
                {
                    unitObject.GetStateController().TransitionToState(ResourceManager.GetAiState(stateName));
                }
            }
        }

        // update fog of war revealers
        if (fogOfWar)
        {
            fogOfWar.SetRevealers(new List<WorldObject>(GetComponentsInChildren<WorldObject>()));
        }

        return unitObject;
    }

    public Building AddBuilding(string name, Vector3 position, Quaternion rotation, string objectName = "")
    {
        GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetBuilding(name));
        Building building = newObject.GetComponent<Building>();
        building.transform.position = position;
        building.transform.rotation = rotation;
        building.transform.parent = buildingsWrapper.transform;
        building.SetPlayer();
        building.SetTeamColor();

        if (objectName != "")
        {
            building.objectName = objectName;
        }

        Building buildingObject = newObject.GetComponent<Building>();

        if (buildingObject)
        {
            buildingObject.ObjectId = ResourceManager.GetNewObjectId();
        }

        // update fog of war revealers
        if (fogOfWar)
        {
            fogOfWar.SetRevealers(new List<WorldObject>(GetComponentsInChildren<WorldObject>()));
        }

        building.UpdateChildRenderers();
        building.CalculateBounds();

        return building;
    }

    public static WorldObject GetObjectById(int id)
    {
        WorldObject[] objects = GameObject.FindObjectsOfType(typeof(WorldObject)) as WorldObject[];
        foreach (WorldObject obj in objects)
        {
            if (obj.ObjectId == id) return obj;
        }
        return null;
    }

    public PlayerData GetData ()
    {
        var data = new PlayerData();

        data.selectedObjectId = SelectedObject ? SelectedObject.ObjectId : -1;
        data.selectedObjectIds = selectedObjects.Select(obj => obj ? obj.ObjectId : -1).ToList();
        data.teamColor = teamColor;
        data.username = username;
        data.human = human;
        data.lockCursor = lockCursor;
        data.units = units.Select(unit => unit.GetData()).ToList();
        data.buildings = buildings.Select(building => building.GetData()).ToList();

        var stateController = GetComponent<Events.StateController>();
        if (stateController)
        {
            data.eventStateController = stateController.GetData();
        }

        return data;
    }

    public void SetData (PlayerData data)
    {
        Start();

        units.Where(unit => unit.ObjectId <= 0).ToList().ForEach(unit => Destroy(unit.gameObject));
        buildings.Where(building => building.ObjectId <= 0).ToList().ForEach(building => Destroy(building.gameObject));
        Debug.Log("Player existing units and buildings were succefully destroyed");

        units = data.units
            .Where(unit => !Player.GetObjectById(unit.objectId))
            .Select(unit =>
            {
                GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetUnit(unit.type), unit.position, unit.rotation);
                Unit createdUnit = newObject.GetComponent<Unit>();
                createdUnit.SetData(unit);
                createdUnit.transform.parent = unitsWrapper.transform;
                createdUnit.SetPlayer();
                createdUnit.SetTeamColor();

                return createdUnit;
            }).ToList();

        buildings = data.buildings
            .Where(building => !Player.GetObjectById(building.objectId))
            .Select(building =>
            {
                GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetBuilding(building.type), building.position, building.rotation);
                Building createdBuilding = newObject.GetComponent<Building>();
                createdBuilding.SetData(building);
                createdBuilding.transform.parent = buildingsWrapper.transform;
                createdBuilding.SetPlayer();
                createdBuilding.SetTeamColor();

                return createdBuilding;
            }).ToList();

        lockCursor = data.lockCursor;
        human = data.human;
        username = data.username;
        teamColor = data.teamColor;
        SelectedObject = data.selectedObjectId != -1
            ? GetObjectById(data.selectedObjectId)
            : null;
        selectedObjects = data.selectedObjectIds.Select(id => id != -1 ? GetObjectById(id) : null).ToList();

        if (data.eventStateController != null)
        {
            var stateController = GetComponent<Events.StateController>();
            stateController.SetData(data.eventStateController);
        }

        Start();
    }
}