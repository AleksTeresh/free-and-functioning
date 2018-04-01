using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/AbilityMulti")]
public class AbilityMultiAction : Action
{

    public override void Act(StateController controller)
    {
        AttackMulti(controller);
    }

    private void AttackMulti(StateController controller)
    {
        Unit unit = controller.unit;
        Ability abilityToUse = controller.abilityToUse;

        if (abilityToUse.isAoe)
        {
            // for now, all AoE are self-AoE
            controller.unit.UseAbilityOnArea(unit.transform.position, abilityToUse);
            controller.abilityToUse = null;

            return;
        }

        // if cannot be used on multiple targets, return
        if (!abilityToUse.isMultiTarget)
        {
            return;
        }

        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> reachableEnemies = controller.nearbyEnemies
            .Where(p =>
            {
                Vector3 currentEnemyPosition = p.transform.position;
                Vector3 direction = currentEnemyPosition - currentPosition;

                return direction.sqrMagnitude < unit.weaponRange * unit.weaponRange;
            })
            .ToList();

        if (reachableEnemies.Count > 0)
        {
            controller.unit.UseAbilityMulti(reachableEnemies, abilityToUse);
            controller.abilityToUse = null;
        }
    }
}
