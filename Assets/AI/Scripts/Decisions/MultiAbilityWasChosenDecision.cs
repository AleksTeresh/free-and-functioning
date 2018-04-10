using System.Collections.Generic;
using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Decisions/MultiAbilityWasChosenDecision")]
public class MultiAbilityWasChosenDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.abilityToUse != null &&
            new List<Ability>(controller.unit.abilitiesMulti).Contains(controller.abilityToUse);
    }
}
