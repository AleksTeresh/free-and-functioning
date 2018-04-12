﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Abilities;
using RTS;

namespace AI.CCEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/CCEnemy/ChooseSleepAbility")]
    public class ChooseSleepAbilityAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(StateController controller)
        {
            Unit unit = controller.unit;

            Ability ability = AbilityUtils.FindAbilityByName("CCEnemySleepMultiAbility", unit.abilitiesMulti);

            // TODO: add logic on when to use Sleep Multi.
        }
    }
}