using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/UseAbilityOnAllies")]
public class UseAbilityOnAlliesAction : UnitAction
{
    protected override void DoAction(UnitStateController controller)
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
