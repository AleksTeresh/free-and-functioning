using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/AbilityWasChosenDecision")]
public class AbilityWasChosenDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.abilityToUse != null;
    }
}
