using UnityEngine;
using Abilities;
using RTS;

namespace AI.DDEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/DDEnemy/ChooseAbilityToUse")]
    public class ChooseAbilityToUseAction : Action
    {
        public override void Act(StateController controller)
        {
            ChooseAbilityToUse(controller);
        }

        private void ChooseAbilityToUse(StateController controller)
        {
            Unit unit = controller.unit;

            EnemyStateController enemyStateController = (EnemyStateController)controller;

            if (!enemyStateController.abilityToUse && enemyStateController.IsReadyToChooseAbility())
            {
                Ability ability = AbilityUtils.ChooseRandomReadyAbility(unit.abilities);

                if (ability != null)
                {
                    if (ability is AoeAbility)
                    {
                        controller.aoeAbilityTarget = controller.chaseTarget.transform.position;
                    }
                    
                    controller.abilityToUse = ability;
                    enemyStateController.ResetAbilityChoiceTimer();
                }
            }
        }
    }
}
