using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

[CreateAssetMenu (menuName = "AI/Actions/AbilityChase")]
public class AbilityChaseAction : Action {
	public override void Act (StateController controller)
	{
		Chase(controller);
	}

	private void Chase(StateController controller)
	{
		Unit unit = controller.unit;
		WorldObject chaseTarget = controller.chaseTarget;
		if (chaseTarget && !WorkManager.ObjectCanReachTargetWithAbility(unit, controller.abilityToUse, chaseTarget.GetFogOfWarAgent()))
		{
			controller.unit.StartMove(chaseTarget.transform.position);
		}
		else
		{
			controller.unit.StopMove();
		}
	} 
}
