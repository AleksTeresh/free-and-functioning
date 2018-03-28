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
        if (controller.chaseTarget)
        {
            Vector3 attackPosition = WorkManager.FindNearestAttackPosition(controller.unit, controller.chaseTarget);
            controller.unit.StartMove(attackPosition);
        }
        
        controller.navMeshAgent.isStopped = false;
    }

    
}
