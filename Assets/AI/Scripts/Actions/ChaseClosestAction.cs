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

            var chaseTarget = AttackUtil.SetClosestEnemyAsTarget(controller);

            if (chaseTarget && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, chaseTarget))
            {
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
