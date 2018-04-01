using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/AbilityWasUsedDecision")]
public class AbilityWasUsedDecision : Decision {

	public override bool Decide (StateController controller)
	{
        bool isAiming = controller.unit.aiming;

		return controller.abilityToUse == null && !isAiming ? true : false;
	}
}
