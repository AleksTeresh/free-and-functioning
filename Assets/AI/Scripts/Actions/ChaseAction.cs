using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/Chase")]
    public class ChaseAction : UnitAction
    {
        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            if (!unit) return;

            WorldObject chaseTarget = controller.chaseTarget;
            if (chaseTarget && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, chaseTarget))
            {
                float heightDiff = Mathf.Abs(unit.transform.position.y - chaseTarget.transform.position.y);
                var idealClosestPoint = WorkManager.GetTargetClosestPoint(unit, chaseTarget);

                // if the destination is still the same, do not recalculate the path
                if (idealClosestPoint == unit.GetNavMeshAgent().destination) return;

                var actualClosestPoint = WorkManager.GetClosestPointOnNavMesh(idealClosestPoint, "Walkable", heightDiff + 3);

                // if the destination is still the same, do not recalculate the path
                if (actualClosestPoint.HasValue && actualClosestPoint != unit.GetNavMeshAgent().destination)
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

}
