using System.Collections.Generic;
using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Decisions/SingleAbilityWasChosenDecision")]
    public class SingleAbilityWasChosenDecision : UnitDecision
    {
        protected override bool DoDecide(UnitStateController controller)
        {
            return controller.abilityToUse != null &&
                !(controller.abilityToUse is AoeAbility) &&
                new List<Ability>(controller.unit.GetAbilityAgent().abilities)
                    .Contains(controller.abilityToUse);
        }
    }
}
    
