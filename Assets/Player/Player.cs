using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class Player : MonoBehaviour {

    public HUD hud;
    public WorldObject SelectedObject { get; set; }
    public Color teamColor;

    public string username;
    public bool human;

    // Use this for initialization
    void Start () {
        hud = GetComponentInChildren<HUD>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation, Building creator)
    {
        Units units = GetComponentInChildren<Units>();
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
        newUnit.transform.parent = units.transform;
        Unit unitObject = newUnit.GetComponent<Unit>();

        if (unitObject)
        {
            unitObject.Init(creator);
            if (spawnPoint != rallyPoint) unitObject.StartMove(rallyPoint);
        }
    }
}
