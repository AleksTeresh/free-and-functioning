﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Abilities;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/BasicAbilityToMulti")]
    public class BasicAbilityToMultiAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(UnitStateController controller)
        {
            Unit unit = controller.unit;

            var abilities = new List<Ability>(unit.GetAbilityAgent().abilitiesMulti);

            if (controller.chaseTarget)
            {
                var currentPosition = unit.transform.position;

                abilities.ForEach(ability =>
                {
                    if (!ability.IsReady()) return;

                    var reachableEnemies = WorkManager.FindReachableObjects(controller.nearbyEnemies, currentPosition, ability.range);

                    // use the ability if there are at least 2 enemies it is going to affect,
                    // or if the indicatedObject is close enough to a single enemy to start ordninary attack
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
}
