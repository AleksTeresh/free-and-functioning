using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/EnemyIsFarEnough")]
public class EnemyIsFarEnoughDecision : Decision
{
    private static readonly float STOP_RUNNING_COEF = 0.8f;

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

        bool targetIsFarEnough = target != null && // target exists
            target.gameObject.activeSelf && // target is alive
            target is MeleeUnit && // target is a MeleeUnit,so it makes sense to kite it
            direction.sqrMagnitude > self.weaponRange * self.weaponRange * STOP_RUNNING_COEF * STOP_RUNNING_COEF; // target is far enough to stop running away

        return targetIsFarEnough;
    }
}