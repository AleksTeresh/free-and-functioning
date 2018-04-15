using UnityEngine;
using Abilities;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseAbilityToUse")]
public class ChooseAbilityAction : UnitAction
{
    public override void Act(StateController baseController)
    {
        if (!(baseController is UnitStateController)) return;

        var controller = (UnitStateController)baseController;

        if (controller.abilityToUse != null)
        {
            return;
        }

        ChooseAbilityToUse(controller);
    }

    protected virtual void ChooseAbilityToUse(UnitStateController controller)
    {
        // the method is to be overriden
    }
}
