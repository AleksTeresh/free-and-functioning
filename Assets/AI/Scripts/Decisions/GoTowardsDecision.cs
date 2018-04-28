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
            target.gameObject.activeSelf &&
            (!(target is Unit) || (target.GetFogOfWarAgent() && target.GetFogOfWarAgent().IsObserved())); // if unit, it should be visible

        return chaseTargetIsActive;
    }
}
