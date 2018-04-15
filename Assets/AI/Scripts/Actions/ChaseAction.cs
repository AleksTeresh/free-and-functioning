using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu (menuName = "AI/Actions/Chase")]
public class ChaseAction : UnitAction {
    protected override void DoAction(UnitStateController controller)
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
