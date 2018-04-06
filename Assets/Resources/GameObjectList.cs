using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Statuses;

public class GameObjectList : MonoBehaviour {

    private static bool created = false;

    public GameObject[] buildings;
    public GameObject[] units;
    public GameObject[] worldObjects;
    public GameObject player;
    public Texture2D[] avatars;
    public State[] aiStates;
    public GameObject[] uiElements;
    public GameObject[] statuses;
	public ParticleSystem[] abilityVFX;

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

		throw new UnregisteredAssetException (GetErrorMessage("Building", name));
    }

    public GameObject GetUnit(string name)
    {
        for (int i = 0; i < units.Length; i++)
        {
            Unit unit = units[i].GetComponent<Unit>();
            if (unit && unit.name == name) return units[i];
        }
        
		throw new UnregisteredAssetException (GetErrorMessage("Unit", name));
    }

    public GameObject GetWorldObject(string name)
    {
        foreach (GameObject worldObject in worldObjects)
        {
            if (worldObject.name == name) return worldObject;
        }
        
		throw new UnregisteredAssetException (GetErrorMessage("World Object", name));
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
        
		throw new UnregisteredAssetException (GetErrorMessage("Build Image", name));
    }

    public GameObject GetStatus(string name)
    {
        foreach (GameObject status in statuses)
        {
            if (status.name == name) return status;
        }

		throw new UnregisteredAssetException (GetErrorMessage("Status", name));
    }

    public State GetAiState(string name)
    {
        foreach (State state in aiStates)
        {
            if (state.name == name) return state;
        }

		throw new UnregisteredAssetException (GetErrorMessage("AI State", name));
    }

    public GameObject GetUIElement(string name)
    {
        foreach (GameObject element in uiElements)
        {
            if (element.name == name) return element;
        }
        
		throw new UnregisteredAssetException (GetErrorMessage("UI Element", name));

    }

	public ParticleSystem GetAbilityVfx(string name)
	{
		foreach (ParticleSystem vfx in abilityVFX) {
			if (vfx.name == name) return vfx;
		}

		throw new UnregisteredAssetException (GetErrorMessage("Ability VFX", name));
	}

	private string GetErrorMessage(string prefabType, string assetName) 
	{
		return string.Format ("{0} prefab {1} is not registered", prefabType, assetName);
	}
}

internal class UnregisteredAssetException : System.Exception 
{
	public UnregisteredAssetException() : base() { }
	public UnregisteredAssetException(string message) : base(message) { }
	public UnregisteredAssetException(string message, System.Exception inner) : base(message, inner) { }

	// A constructor is needed for serialization when an
	// exception propagates from a remoting server to the client. 
	protected UnregisteredAssetException(System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) { }
}
