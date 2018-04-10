using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/BasicAbilityToMulti")]
public class BasicAbilityToMultiAction : Action
{

    public override void Act(StateController controller)
    {
        UseAbility(controller);
    }

    private void UseAbility(StateController controller)
    {
        Unit unit = controller.unit;

        var abilities = new List<Ability>(unit.abilitiesMulti);

        if (controller.chaseTarget)
        {
            var currentPosition = unit.transform.position;

            abilities.ForEach(ability =>
            {
                if (!ability.isReady) return;

                var reachableEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, currentPosition, ability.range);

                // use the ability if there are at least 2 enemies it is going to affect,
                // or if the unit is close enough to a single enemy to start ordninary attack
                if (
                    reachableEnemies.Count > 1 ||
                    (
                        reachableEnemies.Count == 1 &&
                        (reachableEnemies[0].transform.position - currentPosition).sqrMagnitude <= unit.weaponRange * unit.weaponRange)
                    )
                {
                    controller.abilityToUse = ability;
                }
            });
        }
    }
}