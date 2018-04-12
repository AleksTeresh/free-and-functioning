using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;

[CreateAssetMenu(menuName = "AI/Actions/UseAbilityOnAllies")]
public class UseAbilityOnAlliesAction : Action
{
    public override void Act(StateController controller)
    {
        UseAbility(controller);
    }

    private void UseAbility(StateController controller)
    {
        Unit unit = controller.unit;
        Ability abilityToUse = controller.abilityToUse;

        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> reachableAllies = controller.nearbyAllies
            .Where(p =>
            {
                Vector3 currentAllyPosition = p.transform.position;
                Vector3 direction = currentAllyPosition - currentPosition;

                return direction.sqrMagnitude < unit.weaponRange * unit.weaponRange;
            })
            .ToList();

        if (reachableAllies.Count > 0)
        {
            controller.unit.UseAbility(reachableAllies, abilityToUse);
            controller.abilityToUse = null;
        }
    }
}
