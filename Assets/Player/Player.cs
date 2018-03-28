using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class Player : MonoBehaviour {

    public WorldObject SelectedObject { get; set; }
    public Color teamColor;

    public string username;
    public bool human;

    private Units units;
    private Buildings buildings;
    private HUD hud;

    // Use this for initialization
    void Start () {
        hud = GetComponentInChildren<HUD>();
        units = GetComponentInChildren<Units>();
        buildings = GetComponentInChildren<Buildings>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsDead()
    {
        Building[] buildings = GetComponentsInChildren<Building>();
        Unit[] units = GetComponentsInChildren<Unit>();
        if (buildings != null && buildings.Length > 0) return false;
        if (units != null && units.Length > 0) return false;
        return true;
    }

    public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator)
    {
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = units.transform;
        Unit unitObject = newUnit.GetComponent<Unit>();

        if (unitObject)
        {
            unitObject.Init(creator);
            unitObject.ObjectId = ResourceManager.GetNewObjectId();
            if (spawnPoint != rallyPoint) unitObject.StartMove(rallyPoint);
        }
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
                    building.transform.parent = buildings.transform;
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
                    unit.transform.parent = units.transform;
                    unit.SetPlayer();
                    unit.SetTeamColor();
                }
            }
            else if (reader.TokenType == JsonToken.EndArray) return;
        }
    }
}
