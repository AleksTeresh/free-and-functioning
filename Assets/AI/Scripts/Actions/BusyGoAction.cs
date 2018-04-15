using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/BusyGo")]
public class BusyGoAction : UnitAction
{
    protected override void DoAction(UnitStateController controller)
    {
        var self = controller.navMeshAgent;

        controller.unit.StartMove(self.destination);
    }
}