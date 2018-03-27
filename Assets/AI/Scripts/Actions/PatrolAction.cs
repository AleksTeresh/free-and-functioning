using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : Action {

	public override void Act (StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        var self = controller.navMeshAgent;
        controller.unit.StartMove(controller.wayPointList[controller.nextWayPoint].position);

        if (self.remainingDistance <= self.stoppingDistance && !self.pathPending)
        {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
