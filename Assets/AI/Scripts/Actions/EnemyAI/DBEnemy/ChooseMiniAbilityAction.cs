using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;

namespace AI.DBEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/DBEnemy/ChooseMiniAbility")]
    public class ChooseMiniAbilityAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(UnitStateController controller)
        {
            Unit unit = controller.unit;

            Ability ability = AbilityUtils.FindAbilityByName("DBEnemyMiniDebuffAoeAbility", unit.GetAbilityAgent().abilitiesMulti);
            var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

            if (ability != null && ability.IsReady() && reachabeEnemies.Count > 0)
            {
                var abilityTargets = reachabeEnemies;

                controller.aoeAbilityTarget = AoeUtils.GetAoeTargetPosition(
                    ability.range,
                    abilityTargets.ToList(),
                    unit.GetPlayer()
                );
                controller.abilityToUse = ability;
            }
        }
    }
}