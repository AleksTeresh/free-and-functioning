using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/ChooseIdleStateType")]
public class ChooseIdleStateTypeDecision : Decision {

	public override bool Decide (StateController controller)
	{
		return controller.targetManager.InMultiMode;
	}
}
