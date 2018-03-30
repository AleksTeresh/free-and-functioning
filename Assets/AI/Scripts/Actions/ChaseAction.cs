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
        Unit unit = controller.unit;
        WorldObject chaseTarget = controller.chaseTarget;
        if (chaseTarget && !WorkManager.ObjectCanReachTarget(unit, chaseTarget.GetFogOfWarAgent()))
        {
            controller.unit.StartMove(chaseTarget.transform.position);
        }
        else
        {
            controller.unit.StopMove();
        }
    } 
}
