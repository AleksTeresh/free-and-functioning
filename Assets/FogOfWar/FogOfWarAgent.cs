﻿using Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarAgent : MonoBehaviour {
    private WorldObject relatedObject;
    private List<MeshRenderer> meshRenderers;
    private FogOfWar fogOfWar;

    private bool isObserved;

	// Use this for initialization
	void Start () {
        meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        fogOfWar = FindObjectOfType<FogOfWar>();
        relatedObject = GetComponent<WorldObject>();

        isObserved = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (fogOfWar && fogOfWar.IsFogUpToDate())
        {
            bool[] revealedPixels = fogOfWar.GetRevealedPixels();
            bool[] discoveredPixels = fogOfWar.GetDiscoveredPixels();

            var position = transform.position;
            int x = Mathf.RoundToInt(position.x * (fogOfWar.GetTextureWidth() / fogOfWar.GetMapSize().x));
            int y = Mathf.RoundToInt(position.z * (fogOfWar.GetTextureHeight() / fogOfWar.GetMapSize().y));

            int index = x + y * fogOfWar.GetTextureWidth();

            if (meshRenderers.Count > 0)
            {
                if (relatedObject && (relatedObject is Unit || relatedObject.belongsToBoss) && revealedPixels[index] != meshRenderers[0].enabled)
                {
                    meshRenderers.ForEach(p => p.enabled = revealedPixels[index]);
                    relatedObject.UpdateChildRenderers();
                    relatedObject.CalculateBounds();
                }
                else if (relatedObject && !(relatedObject is Unit || relatedObject.belongsToBoss) && discoveredPixels[index] != meshRenderers[0].enabled)
                {
                    meshRenderers.ForEach(p => p.enabled = discoveredPixels[index]);
                    relatedObject.UpdateChildRenderers();
                    relatedObject.CalculateBounds();
                }
                else if (!relatedObject && discoveredPixels[index] != meshRenderers[0].enabled)
                {
                    meshRenderers.ForEach(p => p.enabled = discoveredPixels[index]);
                }
            }

            isObserved = revealedPixels[index];
        }
	}

    public bool IsObserved()
    {
        return isObserved;
    }

    public FogOfWarAgentData GetData()
    {
        var data = new FogOfWarAgentData();

        data.isObserved = isObserved;

        return data;
    }

    public void SetData (FogOfWarAgentData data)
    {
        isObserved = data.isObserved;
    }
}
