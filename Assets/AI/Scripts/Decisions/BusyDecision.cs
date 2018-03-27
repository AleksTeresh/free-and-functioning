using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Busy")]
public class BusyDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        var self = controller.navMeshAgent;

        return self.remainingDistance > self.stoppingDistance || self.pathPending;
    }
}
