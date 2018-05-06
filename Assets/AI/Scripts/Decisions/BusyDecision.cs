using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Decisions/Busy")]
    public class BusyDecision : UnitDecision
    {

        protected override bool DoDecide(UnitStateController controller)
        {
            var self = controller.navMeshAgent;

            return self.remainingDistance > self.stoppingDistance || self.pathPending;
        }
    }

}
