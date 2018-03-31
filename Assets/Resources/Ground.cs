using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        
    }

    public Material GetMaterial()
    {
        Material material = new Material(Shader.Find("Custom/FogOfWar"));
        Terrain terrain = GetComponent<Terrain>();
        terrain.materialType = Terrain.MaterialType.Custom;
        terrain.materialTemplate = material;

        return material;
    }
}
