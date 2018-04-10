using System.Collections.Generic;
using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Decisions/SingleAbilityWasChosenDecision")]
public class SingleAbilityWasChosenDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.abilityToUse != null &&
            !(controller.abilityToUse is AoeAbility) &&
            new List<Ability>(controller.unit.abilities).Contains(controller.abilityToUse);
    }
}
