using UnityEngine;
using Abilities;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/ChooseAbilityToUse")]
    public class ChooseAbilityToUseAction : ChooseAbilityAction
    {
        protected override void ChooseAbilityToUse(UnitStateController controller)
        {
            Unit unit = controller.unit;

            if (!controller.abilityToUse)
            {
                Ability ability = unit.GetAbilityAgent().GetFirstReadyAbility();

                if (ability != null)
                {
                    controller.abilityToUse = ability;
                }
            }
        }
    }

}
