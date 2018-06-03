using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class MinimapCamera : MonoBehaviour {

    private Collider voidPlaneCollider;
    private Camera minimapCamera;

    private LineRenderer line1;
    private LineRenderer line2;
    private LineRenderer line3;
    private LineRenderer line4;

    private void Start()
    {
        voidPlaneCollider = FindObjectOfType<Void>().GetComponent<Collider>();
        minimapCamera = Camera.allCameras.First(p => p.name == "MinimapCamera");

        GameObject myLine = new GameObject();
        myLine.transform.position = minimapCamera.transform.position - new Vector3(minimapCamera.orthographicSize, 0, minimapCamera.orthographicSize);
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        var lines = GetComponentsInChildren<LineRenderer>();
        line1 = lines[0];
        line2 = lines[1];
        line3 = lines[2];
        line4 = lines[3];
    }

    void OnPostRender()
    {
        List<Vector3?> vertices = CameraManager.CalculateMapFrustum(
            minimapCamera,
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
            line1.enabled = true;
            line1.SetPosition(0, vertices[3].Value);
            line1.SetPosition(1, vertices[2].Value);

            line2.enabled = true;
            line2.SetPosition(0, vertices[2].Value);
            line2.SetPosition(1, vertices[1].Value);

            line3.enabled = true;
            line3.SetPosition(0, vertices[1].Value);
            line3.SetPosition(1, vertices[0].Value);

            line4.enabled = true;
            line4.SetPosition(0, vertices[0].Value);
            line4.SetPosition(1, vertices[3].Value);
        }
        else
        {
            line1.enabled = false;
            line2.enabled = false;
            line3.enabled = false;
            line4.enabled = false;
        }
    }
}
