using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/AbilityWasUsedDecision")]
public class AbilityWasUsedDecision : Decision {

	public override bool Decide (StateController controller)
	{
		return controller.abilityToUse == null ? true : false;
	}
}
