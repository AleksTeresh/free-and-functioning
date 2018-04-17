using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu (menuName = "AI/Actions/AbilityChase")]
public class AbilityChaseAction : UnitAction {
    protected override void DoAction(UnitStateController controller)
    {
		Unit unit = controller.unit;
		WorldObject chaseTarget = controller.chaseTarget;
		if (chaseTarget && !WorkManager.ObjectCanReachTargetWithAbility(unit, controller.abilityToUse, chaseTarget))
		{
			controller.unit.StartMove(chaseTarget.transform.position);
		}
		else
		{
			controller.unit.StopMove();
		}
	} 
}
