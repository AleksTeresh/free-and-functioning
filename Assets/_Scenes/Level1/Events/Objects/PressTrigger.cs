using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class PressTrigger : MonoBehaviour {
    public float radius = 10;
    public int requiredNumberOfUnits = 3;


    public bool IsPressed ()
    {
        var pressingUnits = WorkManager.FindNearbyUnits(transform.position, radius);

        return pressingUnits.Count >= requiredNumberOfUnits;
    }
}
