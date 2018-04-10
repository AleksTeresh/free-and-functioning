using System.Collections.Generic;
using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Decisions/AoeAbilityWasChosenDecision")]
public class AoeAbilityWasChosenDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.abilityToUse != null &&
            controller.abilityToUse is AoeAbility;
    }
}
