using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTS;
using Abilities;
using UnityEngine;

namespace AI.DBEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/DBEnemy/ChooseDotAbility")]
    public class ChooseDotAbilityAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(StateController controller)
        {
            Unit unit = controller.unit;

            Ability ability = AbilityUtils.FindAbilityByName("DBEnemyDotAbilityMulti", unit.abilitiesMulti);
            var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

            // wait till at least 2 targets are reachable
            if (ability != null && ability.IsReady() && reachabeEnemies.Count > 1)
            {
                controller.abilityToUse = ability;
            }
        }
    }
}