using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlockingWall : Building {

    /*
    private bool activated = false;
    private Renderer[] meshes;

    protected override void Start()
    {
        base.Start();

        meshes = GetComponentsInChildren<Renderer>();

        if (meshes != null)
        {
            foreach (var mesh in meshes)
            {
                mesh.enabled = false;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!activated)
        {

            if (meshes != null)
            {
                foreach (var mesh in meshes)
                {
                    mesh.enabled = false;
                }
            }
        }
    }

    public void ActivateBlock ()
    {
        if (meshes != null)
        {
            foreach (var mesh in meshes)
            {
                mesh.enabled = true;
            }
        }

        UpdateChildRenderers();
        CalculateBounds();

        var navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();

        if (navMeshObstacle)
        {
            navMeshObstacle.enabled = true;
        }
    }

    */
}
