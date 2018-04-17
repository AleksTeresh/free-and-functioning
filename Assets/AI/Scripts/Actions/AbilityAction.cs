﻿using UnityEngine;
using Abilities;
using AI;
using RTS;

[CreateAssetMenu (menuName = "AI/Actions/Ability")]
public class AbilityAction : UnitAction {

    protected override void DoAction(UnitStateController controller)
    {
		Unit unit = controller.unit;
		WorldObject chaseTarget = controller.enemyAbilityTarget
            ? controller.enemyAbilityTarget
            : controller.chaseTarget;
		Ability ability = controller.abilityToUse;

        if (!ability)
        {
            return;
        }

        if (ability is AoeAbility)
        {
            AoeAbility aoeAbility = (AoeAbility)ability;
            // for now, all AoE are self-AoE
            controller.unit.UseAbility(unit.transform.position, aoeAbility);
            controller.abilityToUse = null;

            return;
        }

        // if no target or canтot attack, return
        if (!ability || chaseTarget == null || !unit.CanAttack())
		{
			return;
		}

		Vector3 currentPosition = unit.transform.position;
		Vector3 currentEnemyPosition = WorkManager.GetTargetClosestPoint(unit, chaseTarget);
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
