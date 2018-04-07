using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/ChangeTargetToMostVulnerable")]
public class ChangeTargetToMostVulnerableAction : Action
{
    public override void Act(StateController controller)
    {
        SelectMostVulnerable(controller);
    }

    private void SelectMostVulnerable(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        WorldObject chaseTarget = controller.chaseTarget;

        WorldObject mostVulnerableEnemy = WorkManager.FindMostVulnerableObjectInList(controller.nearbyEnemies);

        if (mostVulnerableEnemy)
        {
            controller.chaseTarget = mostVulnerableEnemy;

            if (!WorkManager.ObjectCanReachTarget(unit, mostVulnerableEnemy.GetFogOfWarAgent()))
            {
                controller.unit.StartMove(mostVulnerableEnemy.transform.position);
            }
            else
            {
                controller.unit.StopMove();
            }
        }
    }
}
