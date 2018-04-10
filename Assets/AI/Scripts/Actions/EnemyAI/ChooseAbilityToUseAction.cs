using UnityEngine;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseAbilityToUse")]
public class ChooseAbilityToUseAction : ChooseAbilityAction
{ 
    protected override void ChooseAbilityToUse(StateController controller)
    {
        Unit unit = controller.unit;

        if (!controller.abilityToUse)
        {
            Ability ability = unit.GetFirstReadyAbility();

            if (ability != null)
            {
                controller.abilityToUse = ability;
            }
        }
    }
}
