using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/AbilityChase")]
    public class AbilityChaseAction : UnitAction
    {
        public override bool IsExpensive()
        {
            return true;
        }

        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            WorldObject chaseTarget = controller.chaseTarget;
            if (chaseTarget && !WorkManager.ObjectCanReachTargetWithAbility(unit, controller.abilityToUse, chaseTarget))
            {
                var newDestination = WorkManager.FindDistinationPointByTarget(chaseTarget, unit);

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
