using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

[CreateAssetMenu (menuName = "AI/Actions/Chase")]
public class ChaseAction : Action {
    public override void Act (StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        WorldObject chaseTarget = controller.chaseTarget;
        if (chaseTarget)
        {
            Vector3 attackPosition = WorkManager.FindNearestAttackPosition(controller.unit, chaseTarget);
            controller.unit.StartMove(attackPosition);
        }
        
        controller.navMeshAgent.isStopped = false;
    }

    
}
