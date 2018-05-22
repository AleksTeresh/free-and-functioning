using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadingRotator : MonoBehaviour {
    private static readonly float ROTATOR_SPEED = 100;

    RectTransform rotatorTransform;
    // Use this for initialization
    void Start () {
        rotatorTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rotatorTransform)
        {
            rotatorTransform.Rotate(Vector3.forward, ROTATOR_SPEED * Time.deltaTime);
        }
    }
}
