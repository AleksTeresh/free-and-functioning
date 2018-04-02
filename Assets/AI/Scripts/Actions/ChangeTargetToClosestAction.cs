using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/ChangeTargetToClosest")]
public class ChangeTargetToClosestAction : Action
{
    public override void Act(StateController controller)
    {
        ChangeTargetToClosest(controller);
    }

    private void ChangeTargetToClosest(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        WorldObject chaseTarget = controller.chaseTarget;

        WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

        if (closestEnemy)
        {
            controller.chaseTarget = closestEnemy;

            if (!WorkManager.ObjectCanReachTarget(unit, closestEnemy.GetFogOfWarAgent()))
            {
                controller.unit.StartMove(closestEnemy.transform.position);
            }
            else
            {
                controller.unit.StopMove();
            }
        }
    }
}