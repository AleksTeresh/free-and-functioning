using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseMultiAbilityToUse")]
public class ChooseMultiAbilityToUseAction : Action
{
    public override void Act(StateController controller)
    {
        ChooseMultiAbilityToUse(controller);
    }

    private void ChooseMultiAbilityToUse(StateController controller)
    {
        Unit unit = controller.unit;

        if (!controller.abilityToUse)
        {
            Ability ability = unit.GetFirstReadyMultiAbility();

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
