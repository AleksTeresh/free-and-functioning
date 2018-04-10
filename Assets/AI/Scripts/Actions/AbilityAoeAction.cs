using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/AbilityAoe")]
public class AbilityAoeAction : Action
{

    public override void Act(StateController controller)
    {
        UseAbility(controller);
    }

    private void UseAbility(StateController controller)
    {
        Unit unit = controller.unit;
        Ability abilityToUse = controller.abilityToUse;

        if (!abilityToUse || !(abilityToUse is AoeAbility))
        {
            return;
        }

        AoeAbility aoeAbility = (AoeAbility)abilityToUse;

        if (controller.aoeAbilityTarget == new Vector3())
        {
            
            // for now, all AoE are self-AoE
            controller.unit.UseAbility(unit.transform.position, aoeAbility);
            controller.abilityToUse = null;
        }

        Vector3 currentPosition = unit.transform.position;
        Vector3 direction = controller.aoeAbilityTarget - currentPosition;

        if (direction.sqrMagnitude < aoeAbility.range * aoeAbility.range)
        {
            controller.unit.UseAbility(controller.aoeAbilityTarget, aoeAbility);

            if (!unit.aiming)
            {
                controller.abilityToUse = null;
            }
        }
    }
}
