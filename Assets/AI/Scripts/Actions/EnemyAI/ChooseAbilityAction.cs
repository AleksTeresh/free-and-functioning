using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseAbilityToUse")]
public class ChooseAbilityAction : Action
{
    public override void Act(StateController controller)
    {
        if (controller.abilityToUse != null)
        {
            return;
        }

        ChooseAbilityToUse(controller);
    }

    protected virtual void ChooseAbilityToUse(StateController controller)
    {
        // the method is to be overriden
    }
}
