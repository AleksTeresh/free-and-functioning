using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Actions/Chase")]
public class ChaseAction : Action {
    public override void Act (StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        if (controller.chaseTarget)
        {
            Vector3 attackPosition = FindNearestAttackPosition(controller.unit, controller.chaseTarget);
            controller.unit.StartMove(attackPosition);
        }
        
        controller.navMeshAgent.isStopped = false;
    }

    private Vector3 FindNearestAttackPosition(WorldObject self, WorldObject target)
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - self.transform.position;
        float targetDistance = direction.magnitude;
        float distanceToTravel = targetDistance - (0.8f * self.weaponRange);
        return Vector3.Lerp(self.transform.position, targetLocation, distanceToTravel / targetDistance);
    }
}
