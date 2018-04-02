using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;

[CreateAssetMenu (menuName = "AI/Actions/Ability")]
public class AbilityAction : Action {

	public override void Act (StateController controller)
	{
		Attack(controller);
	}

	private void Attack(StateController controller)
	{
		Unit unit = controller.unit;
		WorldObject chaseTarget = controller.chaseTarget;
		Ability ability = controller.abilityToUse;

        if (ability is AoeAbility && !ability.isMultiTarget)
        {
            AoeAbility aoeAbility = (AoeAbility)ability;
            // for now, all AoE are self-AoE
            controller.unit.UseAbilityOnArea(unit.transform.position, aoeAbility);
            controller.abilityToUse = null;

            return;
        }

        // if no target or canтot attack, return
        if (chaseTarget == null || !unit.CanAttack())
		{
			return;
		}

		Vector3 currentPosition = unit.transform.position;
		Vector3 currentEnemyPosition = chaseTarget.transform.position;
		Vector3 direction = currentEnemyPosition - currentPosition;

		if (direction.sqrMagnitude < ability.range * ability.range)
		{
			controller.unit.UseAbility(chaseTarget, ability);

            if (!unit.aiming)
            {
                controller.abilityToUse = null;
            }
        }
	}
}
