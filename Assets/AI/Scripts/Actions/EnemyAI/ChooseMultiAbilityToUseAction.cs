﻿using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseMultiAbilityToUse")]
    public class ChooseMultiAbilityToUseAction : UnitAction
    {
        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;

            if (!controller.abilityToUse)
            {
                Ability ability = unit.GetAbilityAgent().GetFirstReadyMultiAbility();

                if (ability != null)
                {
                    if (ability is AoeAbility && controller.chaseTarget)
                    {
                        controller.aoeAbilityTarget = controller.chaseTarget.transform.position;
                    }

                    controller.abilityToUse = ability;
                }
            }
        }
    }
}
