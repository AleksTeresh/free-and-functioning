using UnityEngine;
using Abilities;
using RTS;
using AI;

namespace AI.DDEnemy
{
    [CreateAssetMenu(menuName = "AI/Actions/EnemyAI/DDEnemy/ChooseAbilityToUse")]
    public class ChooseAbilityToUseAction : UnitAction
    {
        protected override void DoAction(UnitStateController controller)
        {
            Unit unit = controller.unit;

            EnemyStateController enemyStateController = (EnemyStateController)controller;

            if (!enemyStateController.abilityToUse && enemyStateController.IsReadyToChooseAbility())
            {
                Ability ability = AbilityUtils.ChooseRandomReadyAbility(unit.GetAbilityAgent().abilities);

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
