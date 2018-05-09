using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/Chase")]
    public class ChaseAction : UnitAction
    {
        public override bool IsExpensive()
        {
            return true;
        }

        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            if (!unit) return;

            WorldObject chaseTarget = controller.chaseTarget;
            if (chaseTarget && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, chaseTarget))
            {
                var newDestination = WorkManager.FindDistinationPointByTarget(chaseTarget, unit);

                if (newDestination.HasValue)
                {
                    unit.StartMove(newDestination.Value);
                }
            }
            else
            {
                unit.StopMove();
            }
        }
    }

}
