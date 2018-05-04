using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu (menuName = "AI/Actions/Chase")]
public class ChaseAction : UnitAction {
    protected override void DoAction(UnitStateController controller)
    {
        Unit unit = controller.unit;
        if (!unit) return;

        WorldObject chaseTarget = controller.chaseTarget;
        if (chaseTarget && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, chaseTarget))
        {
            var idealClosestPoint = WorkManager.GetTargetClosestPoint(unit, chaseTarget);
            var actualClosestPoint = WorkManager.GetClosestPointOnNavMesh(idealClosestPoint, "Walkable", 15);

            if (actualClosestPoint.HasValue)
            {
                unit.StartMove(actualClosestPoint.Value);
            }
        }
        else
        {
            unit.StopMove();
        }
    } 
}
