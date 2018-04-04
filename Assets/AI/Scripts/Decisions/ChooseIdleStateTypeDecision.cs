using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/ChooseIdleStateType")]
public class ChooseIdleStateTypeDecision : Decision {
	public override bool Decide (StateController controller)
	{
        bool canAttackMulti = controller.unit.CanAttackMulti();
        bool isInMultiMode = controller.targetManager.InMultiMode;

		return canAttackMulti ? isInMultiMode : false;
	}
}
