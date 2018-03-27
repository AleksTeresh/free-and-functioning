using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/Look")]
public class LookDecision : Decision {

    public override bool Decide (StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;

    }

    private bool Look (StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, unit.detectionRange);

        if (unit.CanAttack())
        {
            List<WorldObject> enemyObjects = new List<WorldObject>();
            foreach (WorldObject nearbyObject in nearbyObjects)
            {
                if (nearbyObject.GetPlayer() != unit.GetPlayer()) enemyObjects.Add(nearbyObject);
            }
            WorldObject closestObject = WorkManager.FindNearestWorldObjectInListToPosition(enemyObjects, currentPosition);
            if (closestObject)
            {
                controller.chaseTarget = closestObject;

                return true;
            }
        }

        return false;
    }
}
