using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/EnemyIsTooClose")]
public class EnemyIsTooCloseDecision : Decision
{
    private static readonly float RUNAWAY_COEF = 0.2f;

    public override bool Decide(StateController controller)
    {
        Unit self = controller.unit;
        WorldObject target = controller.chaseTarget;

        if (!target)
        {
            return false;
        }

        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - self.transform.position;

        bool targetIsTooClose = target != null && // target exists
            target.gameObject.activeSelf && // target is alive
            target is MeleeUnit && // target is a MeleeUnit,so it makes sense to kite it
            direction.sqrMagnitude < self.weaponRange * self.weaponRange * RUNAWAY_COEF * RUNAWAY_COEF; // target is close enough to start running away

        return targetIsTooClose;
    }
}
