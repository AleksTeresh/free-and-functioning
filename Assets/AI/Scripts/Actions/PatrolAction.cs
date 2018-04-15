using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu (menuName = "AI/Actions/Patrol")]
public class PatrolAction : UnitAction {

    protected override void DoAction(UnitStateController controller)
    {
        var self = controller.navMeshAgent;
        controller.unit.StartMove(controller.wayPointList[controller.nextWayPoint].position);

        if (self.remainingDistance <= self.stoppingDistance && !self.pathPending)
        {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
