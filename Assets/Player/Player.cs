using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;
using Abilities;
using Dialog;

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

    private List<Unit> units;
    private List<Building> buildings;

    private Units unitsWrapper;
    private Buildings buildingsWrapper;
    private HUD hud;
    private FogOfWar fogOfWar;

	void Awake()
	{
		unitMapping = new UnitMapping (hotkeyToUnitNameMapping);
	}
		
    // Use this for initialization
    void Start () {
        hud = GetComponentInChildren<HUD>();
        unitsWrapper = GetComponentInChildren<Units>();
        buildingsWrapper = GetComponentInChildren<Buildings>();
        units = new List<Unit>(GetComponentsInChildren<Unit>());
        buildings = new List<Building>(GetComponentsInChildren<Building>());
        
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

    public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator)
    {
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = unitsWrapper.transform;
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
                    case "Idle Idler":
                        stateName = "Busy Idler";
                        break;

                    case "Idle Vulnerabler":
                        stateName = "Busy Vulnerabler";
                        break;

                    case "Idle Kiter":
                        stateName = "Busy Kiter";
                        break;

                    case "Idle Assassin":
                        stateName = "Busy Assassin";
                        break;

                    case "Idle CCEnemy":
                        stateName = "Busy CCEnemy";
                        break;

                    case "Idle DBEnemy":
                        stateName = "Busy DBEnemy";
                        break;

                    case "Idle EnemyDamageDealer":
                        stateName = "Busy EnemyDamageDealer";
                        break;

                    case "Patrol Chaser":
                        stateName = "Busy Chaser";
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
    }

    public void AddBuilding(string name, Vector3 position, Quaternion rotation, string objectName = "")
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
    }

    public virtual void SaveDetails(JsonWriter writer)
    {
        SaveManager.WriteString(writer, "Username", username);
        SaveManager.WriteBoolean(writer, "Human", human);
        SaveManager.WriteColor(writer, "TeamColor", teamColor);
        // SaveManager.SavePlayerResources(writer, resources);
        SaveManager.SavePlayerBuildings(writer, GetComponentsInChildren<Building>());
        SaveManager.SavePlayerUnits(writer, GetComponentsInChildren<Unit>());
    }

    public void LoadDetails(JsonTextReader reader)
    {
        if (reader == null) return;
        string currValue = "";
        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    currValue = (string)reader.Value;
                }
                else
                {
                    switch (currValue)
                    {
                        case "Username": username = (string)reader.Value; break;
                        case "Human": human = (bool)reader.Value; break;
                        default: break;
                    }
                }
            }
            else if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
            {
                switch (currValue)
                {
                    case "TeamColor": teamColor = LoadManager.LoadColor(reader); break;
                    // case "Resources": LoadResources(reader); break;
                    // TODO: replace with Items
                    case "Buildings": LoadBuildings(reader); break;
                    case "Units": LoadUnits(reader); break;
                    default: break;
                }
            }
            else if (reader.TokenType == JsonToken.EndObject) return;
        }
    }

    public WorldObject GetObjectForId(int id)
    {
        WorldObject[] objects = GameObject.FindObjectsOfType(typeof(WorldObject)) as WorldObject[];
        foreach (WorldObject obj in objects)
        {
            if (obj.ObjectId == id) return obj;
        }
        return null;
    }

    private void LoadBuildings(JsonTextReader reader)
    {
        if (reader == null) return;
        string currValue = "", type = "";
        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (reader.TokenType == JsonToken.PropertyName) currValue = (string)reader.Value;
                else if (currValue == "Type")
                {
                    type = (string)reader.Value;
                    GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetBuilding(type));
                    Building building = newObject.GetComponent<Building>();
                    building.LoadDetails(reader);
                    building.transform.parent = buildingsWrapper.transform;
                    building.SetPlayer();
                    building.SetTeamColor();
                }
            }
            else if (reader.TokenType == JsonToken.EndArray) return;
        }
    }

    private void LoadUnits(JsonTextReader reader)
    {
        if (reader == null) return;
        string currValue = "", type = "";
        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (reader.TokenType == JsonToken.PropertyName) currValue = (string)reader.Value;
                else if (currValue == "Type")
                {
                    type = (string)reader.Value;
                    GameObject newObject = (GameObject)GameObject.Instantiate(ResourceManager.GetUnit(type));
                    Unit unit = newObject.GetComponent<Unit>();
                    unit.LoadDetails(reader);
                    unit.transform.parent = unitsWrapper.transform;
                    unit.SetPlayer();
                    unit.SetTeamColor();
                }
            }
            else if (reader.TokenType == JsonToken.EndArray) return;
        }
    }
}
