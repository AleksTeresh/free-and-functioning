using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AI;
using Abilities;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/AbilityMulti")]
public class AbilityMultiAction : UnitAction
{

    protected override void DoAction(UnitStateController controller)
    {
        Unit unit = controller.unit;
        Ability abilityToUse = controller.abilityToUse;

        if (!abilityToUse)
        {
            return;
        }

        if (abilityToUse is AoeAbility)
        {
            AoeAbility aoeAbility = (AoeAbility)abilityToUse;
            // for now, all AoE are self-AoE
            controller.unit.UseAbility(unit.transform.position, aoeAbility);
            controller.abilityToUse = null;

            return;
        }

        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> reachableEnemies = controller.nearbyEnemies
            .Where(p =>
            {
                Vector3 currentEnemyPosition = WorkManager.GetTargetClosestPoint(unit, p);
                Vector3 direction = currentEnemyPosition - currentPosition;

                return direction.sqrMagnitude < abilityToUse.range * abilityToUse.range;
            })
            .ToList();

        if (reachableEnemies.Count > 0)
        {
            controller.unit.UseAbility(reachableEnemies, abilityToUse);
            controller.abilityToUse = null;
        }
    }
}
