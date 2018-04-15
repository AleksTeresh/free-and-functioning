using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/StayStill")]
public class StayStillAction : UnitAction
{
    protected override void DoAction(UnitStateController controller)
    {
        controller.unit.StopMove();
    }
}
