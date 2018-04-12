using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision {

	public override bool Decide (StateController controller)
    {
        Unit self = controller.unit;
        WorldObject target = controller.chaseTarget;

        if (!target)
        {
            return false;
        }

        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - self.transform.position;

        bool chaseTargetIsActive = target != null && // target exists
            target.gameObject.activeSelf && // target is alive
            (
                direction.sqrMagnitude < self.detectionRange * self.detectionRange || 
                FriendsHaveActiveTarget(target, self, controller.nearbyAllies)
            ); // target is visible to a unit or its friends

        return chaseTargetIsActive;
    }

    private bool FriendsHaveActiveTarget(WorldObject target, WorldObject unit, List<WorldObject> nearbyAllies)
    {
        foreach (WorldObject nearbyAlly in nearbyAllies)
        {
            if (
                nearbyAlly &&
                unit.ObjectId != nearbyAlly.ObjectId &&
                nearbyAlly.GetStateController() &&
                nearbyAlly.GetStateController().chaseTarget == target
            )
            {
                return true;
            }
        }

        return false;
    }
}
