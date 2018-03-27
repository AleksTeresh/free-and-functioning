using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/BusyGo")]
public class BusyGoAction : Action
{

    public override void Act(StateController controller)
    {
        BusyGo(controller);
    }

    private void BusyGo(StateController controller)
    {
        var self = controller.navMeshAgent;

        controller.unit.StartMove(self.destination);
    }
}