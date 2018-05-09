using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/ChaseClosest")]
    public class ChaseClosestAction : UnitAction
    {
        public override bool IsExpensive()
        {
            return true;
        }

        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            Vector3 currentPosition = unit.transform.position;

            WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

            if (closestEnemy && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, closestEnemy))
            {
                controller.chaseTarget = closestEnemy;

                var newDestination = WorkManager.FindDistinationPointByTarget(controller.chaseTarget, unit);

                if (newDestination.HasValue)
                {
                    unit.StartMove(newDestination.Value);
                }
            }
            else
            {
                controller.unit.StopMove();
            }
        }
    }
}
