using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/AbilityWasUsedDecision")]
public class AbilityWasUsedDecision : UnitDecision {

    protected override bool DoDecide(UnitStateController controller)
    {
        bool isAiming = controller.unit.aiming;

		return controller.abilityToUse == null
        /* && !isAiming */
            ? true
            : false;
	}
}
