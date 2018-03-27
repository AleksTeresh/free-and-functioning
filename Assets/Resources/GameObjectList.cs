﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class GameObjectList : MonoBehaviour {

    private static bool created = false;

    public GameObject[] buildings;
    public GameObject[] units;
    public GameObject[] worldObjects;
    public GameObject player;
    public Texture2D[] avatars;
    public State[] aiStates;

    void Awake()
    {
        if (!created)
        {
            PlayerManager.Load();
            PlayerManager.SetAvatarTextures(avatars);

            DontDestroyOnLoad(transform.gameObject);
            ResourceManager.SetGameObjectList(this);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Texture2D[] GetAvatars()
    {
        return avatars;
    }

    public GameObject GetBuilding(string name)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            Building building = buildings[i].GetComponent<Building>();
            if (building && building.name == name) return buildings[i];
        }
        return null;
    }

    public GameObject GetUnit(string name)
    {
        for (int i = 0; i < units.Length; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit && unit.name == name) return units[i];
        }
        return null;
    }

    public GameObject GetWorldObject(string name)
    {
        foreach (GameObject worldObject in worldObjects)
        {
            if (worldObject.name == name) return worldObject;
        }
        return null;
    }

    public GameObject GetPlayerObject()
    {
        return player;
    }

    public Texture2D GetBuildImage(string name)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            Building building = buildings[i].GetComponent<Building>();
            if (building && building.name == name) return building.buildImage;
        }
        for (int i = 0; i < units.Length; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit && unit.name == name) return unit.buildImage;
        }
        return null;
    }

    public State GetAiState(string name)
    {
        foreach (State state in aiStates)
        {
            if (state.name == name) return state;
        }
        return null;
    }
}
