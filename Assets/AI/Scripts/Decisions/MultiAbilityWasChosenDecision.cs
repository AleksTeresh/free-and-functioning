using System.Collections.Generic;
using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Decisions/MultiAbilityWasChosenDecision")]
    public class MultiAbilityWasChosenDecision : UnitDecision
    {
        protected override bool DoDecide(UnitStateController controller)
        {
            return controller.abilityToUse != null &&
                !(controller.abilityToUse is AoeAbility) &&
                new List<Ability>(controller.unit.GetAbilityAgent().abilitiesMulti)
                    .Contains(controller.abilityToUse);
        }
    }
}

