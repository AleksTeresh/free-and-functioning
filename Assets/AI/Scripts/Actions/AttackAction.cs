using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Attack")]
public class AttackAction : Action {

	public override void Act (StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        Unit unit = controller.unit;
        WorldObject chaseTarget = controller.chaseTarget;

        // if no target or canot attack, return
        if (chaseTarget == null || !unit.CanAttack())
        {
            return;
        }

        Vector3 currentPosition = unit.transform.position;
        Vector3 currentEnemyPosition = chaseTarget.transform.position;

        if (
            Vector3.Distance(currentEnemyPosition, currentPosition) <= unit.weaponRange &&
            controller.CheckIfCountDownElapsed(controller.unit.weaponRechargeTime)
        )
        {
            controller.unit.BeginAttack(chaseTarget);
        }
    }
}
