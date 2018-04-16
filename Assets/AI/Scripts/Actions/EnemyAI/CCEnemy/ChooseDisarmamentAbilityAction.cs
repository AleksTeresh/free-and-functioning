using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Abilities;
using RTS;

namespace AI.CCEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/CCEnemy/ChooseDisarmamentAbility")]
    public class ChooseDisarmamentAbilityAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(UnitStateController controller)
        {
            Unit unit = controller.unit;

            Ability ability = AbilityUtils.FindAbilityByName("CCEnemyDisarmamentAoeAbility", unit.abilitiesMulti);
            var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

            if (ability != null && ability.IsReady() && reachabeEnemies.Count > 0)
            {
                controller.aoeAbilityTarget = WorkManager.FindMostDamagingObjectInList(reachabeEnemies).transform.position;
                controller.abilityToUse = ability;
            }
        }
    }
}
