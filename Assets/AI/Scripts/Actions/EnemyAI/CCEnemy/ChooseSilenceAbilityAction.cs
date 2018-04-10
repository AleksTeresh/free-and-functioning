using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/CCEnemy/ChooseSilenceAbility")]
public class ChooseSilenceAbilityAction : ChooseAbilityAction
{
    protected override void ChooseAbilityToUse(StateController controller)
    {
        Unit unit = controller.unit;

        Ability ability = AbilityUtils.FindAbilityByName("CCEnemySilenceAbility", unit.abilities);
        var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

        if (ability != null && ability.IsReady() && reachabeEnemies.Count > 0)
        {
            controller.enemyAbilityTarget = WorkManager.FindMostDamagingObjectInList(reachabeEnemies);
            controller.abilityToUse = ability;
        }
    }
}