using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class MinimapCamera : MonoBehaviour {

    private Collider voidPlaneCollider;

    private void Start()
    {
        voidPlaneCollider = FindObjectOfType<Void>().GetComponent<Collider>();
    }

    void OnPostRender()
    {
        GL.PushMatrix();
        {
            // minimap.material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            {
                List<Vector3?> vertices = CameraManager.CalculateMapFrustum(
                    Camera.allCameras.First(p => p.name == "MinimapCamera"),
                    voidPlaneCollider.GetComponent<Collider>()
                );
                
                // draw only if all the vertices can be drawn
                if (
                    vertices[0].HasValue &&
                    vertices[1].HasValue &&
                    vertices[2].HasValue &&
                    vertices[3].HasValue
                )
                {
                    GL.Vertex(vertices[3].Value);
                    GL.Vertex(vertices[2].Value);
                    GL.Vertex(vertices[2].Value);
                    GL.Vertex(vertices[1].Value);
                    GL.Vertex(vertices[1].Value);
                    GL.Vertex(vertices[0].Value);
                    GL.Vertex(vertices[0].Value);
                    GL.Vertex(vertices[3].Value);
                }
            }
            GL.End();
        }
        GL.PopMatrix();
    }
}
