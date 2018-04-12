using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;

[CreateAssetMenu (menuName = "AI/Actions/AllyAbilityChase")]
public class AllyAbilityChaseAction : Action {

	public override void Act (StateController controller)
	{
		AbilityChase(controller);
	}

	private void AbilityChase(StateController controller)
	{
		Unit unit = controller.unit;
		WorldObject allyChaseTarget = controller.allyAbilityTarget;
		Ability ability = controller.abilityToUse;

		if (allyChaseTarget != null)
		{
			Vector3 currentPosition = unit.transform.position;
			Vector3 currentAllyPosition = allyChaseTarget.transform.position;
			Vector3 direction = currentAllyPosition - currentPosition;

			if (direction.sqrMagnitude < ability.range * ability.range)
			{
				controller.unit.UseAbility(allyChaseTarget, ability);

				if (!unit.aiming)
				{
					controller.abilityToUse = null;
				}
			}
		}
	}
}
