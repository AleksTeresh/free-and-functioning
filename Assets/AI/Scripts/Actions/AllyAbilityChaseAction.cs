using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/AllyAbilityChase")]
    public class AllyAbilityChaseAction : UnitAction
    {
        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;
            WorldObject allyChaseTarget = controller.allyAbilityTarget;
            Ability ability = controller.abilityToUse;

            if (allyChaseTarget != null)
            {
                Vector3 currentPosition = unit.transform.position;
                Vector3 currentAllyPosition = WorkManager.GetTargetClosestPoint(unit, allyChaseTarget);
                Vector3 direction = currentAllyPosition - currentPosition;

                if (direction.sqrMagnitude < ability.range * ability.range)
                {
                    controller.unit.UseAbility(allyChaseTarget, ability);
                    controller.abilityToUse = null;
                    /*
                        if (!unit.aiming)
                        {
                            controller.abilityToUse = null;
                        }
                    */
                }
            }
        }
    }
}

