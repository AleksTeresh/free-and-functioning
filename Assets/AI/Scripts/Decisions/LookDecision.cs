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

        if (unit.CanAttack())
        {
            // if there is a common target, chase it
            if (controller.targetManager && controller.targetManager.SingleTarget)
            {
                controller.chaseTarget = controller.targetManager.SingleTarget;
                return true;
            }

            // if there is no common target, look for nearby enemies
            WorldObject closestObject = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);
            if (closestObject)
            {
                // if there is enemy nearby, and no common target, make the enemy the common target
                if (controller.targetManager && unit.GetPlayer().human)
                {
                    controller.targetManager.SingleTarget = closestObject;
                }
                // make the enemy as own target as well
                controller.chaseTarget = closestObject;

                return true;
            }
        }

        return false;
    }
}
