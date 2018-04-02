using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/StayStill")]
public class StayStillAction : Action
{

    public override void Act(StateController controller)
    {
        StayStill(controller);
    }

    private void StayStill(StateController controller)
    {
        controller.unit.StopMove();
    }
}
