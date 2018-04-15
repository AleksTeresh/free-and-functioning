using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/GoTowards")]
public class GoTowardsDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        var self = controller.controlledObject;
        WorldObject target = controller.chaseTarget;

        bool chaseTargetIsActive = target != null && // target exists
            target.gameObject.activeSelf; // target is alive
        // TODO: add fog of war condition (perhaps)

        return chaseTargetIsActive;
    }
}
