using UnityEngine;
using RTS;
using Events;
using Persistence;

public class PressTrigger : EventObject
{
    public float radius = 10;
    public int requiredNumberOfUnits = 3;

    public bool IsPressed ()
    {
        var pressingUnits = WorkManager.FindNearbyUnits(transform.position, radius);

        return pressingUnits.Count >= requiredNumberOfUnits;
    }

}
