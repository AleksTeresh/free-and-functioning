using System.Collections.Generic;
using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Decisions/AoeAbilityWasChosenDecision")]
    public class AoeAbilityWasChosenDecision : UnitDecision
    {
        protected override bool DoDecide(UnitStateController controller)
        {
            return controller.abilityToUse != null &&
                controller.abilityToUse is AoeAbility;
        }
    }
}

