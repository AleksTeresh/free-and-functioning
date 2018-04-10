using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/CCEnemy/ChooseBindAbility")]
public class ChooseBindAbilityAction : ChooseAbilityAction
{
    protected override void ChooseAbilityToUse(StateController controller)
    {
        Unit unit = controller.unit;

        Ability ability = AbilityUtils.FindAbilityByName("CCEnemyBindAoeAbility", unit.abilitiesMulti);
        var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

        if (ability != null && ability.IsReady() && reachabeEnemies.Find(p => p is MeleeUnit) != null)
        {
            var abilityTargets = reachabeEnemies.Where(p => p is MeleeUnit);

            controller.aoeAbilityTarget = abilityTargets.First().transform.position;
            controller.abilityToUse = ability;
        }
    }
}