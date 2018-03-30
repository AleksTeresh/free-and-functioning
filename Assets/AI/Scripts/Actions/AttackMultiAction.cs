using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/AttackMulti")]
public class AttackMultiAction : Action
{

    public override void Act(StateController controller)
    {
        AttackMulti(controller);
    }

    private void AttackMulti(StateController controller)
    {
        Unit unit = controller.unit;

        // if cannot attack, return
        if (!unit.CanAttackMulti())
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

        controller.attacking = reachableEnemies.Count > 0;
        controller.unit.PerformAttackToMulti(reachableEnemies);
    }
}