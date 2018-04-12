using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/ChaseClosest")]
public class ChaseClosestAction : Action
{

    public override void Act(StateController controller)
    {
        ChaseClosest(controller);
    }

    private void ChaseClosest(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;

        WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

        if (closestEnemy && !WorkManager.ObjectCanReachTarget(unit, closestEnemy.GetFogOfWarAgent()))
        {
            controller.chaseTarget = closestEnemy;
            controller.unit.StartMove(closestEnemy.transform.position);
        }
        else
        {
            controller.unit.StopMove();
        }
    }
}
