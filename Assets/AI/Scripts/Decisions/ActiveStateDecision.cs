using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision {

	public override bool Decide (StateController controller)
    {
        Unit self = controller.unit;
        WorldObject target = controller.chaseTarget;

        bool chaseTargetIsActive = target != null && // target exists
            target.gameObject.activeSelf && // target is alive
            Vector3.Distance(target.transform.position, self.transform.position) <= self.detectionRange; // target is visible

        return chaseTargetIsActive;
    }
}
