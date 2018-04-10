using UnityEngine;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/CCEnemy/ChooseBindAbility")]
public class ChooseBindAbilityAction : Action
{
    public override void Act(StateController controller)
    {
        ChooseBindAbility(controller);
    }

    private void ChooseBindAbility(StateController controller)
    {
        Unit unit = controller.unit;

        Debug.Log("Choosing ability");

        Ability ability = AbilityUtils.FindAbilityByName("CCEnemyBindAoeAbility", unit.abilitiesMulti);
        var reachabeEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, unit.transform.position, ability.range);

        if (ability != null && ability.isReady && WorkManager.FindMeleeObjectInList(reachabeEnemies) != null)
        {
            var abilityTarget = WorkManager.FindMeleeObjectInList(reachabeEnemies);

            controller.enemyAbilityTarget = abilityTarget;
            controller.abilityToUse = ability;
        }
    }
}