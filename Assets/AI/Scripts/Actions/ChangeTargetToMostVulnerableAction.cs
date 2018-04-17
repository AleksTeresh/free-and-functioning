using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/ChangeTargetToMostVulnerable")]
public class ChangeTargetToMostVulnerableAction : UnitAction
{
    protected override void DoAction(UnitStateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        WorldObject chaseTarget = controller.chaseTarget;

        WorldObject mostVulnerableEnemy = WorkManager.FindMostVulnerableObjectInList(controller.nearbyEnemies);

        if (mostVulnerableEnemy)
        {
            controller.chaseTarget = mostVulnerableEnemy;

            if (!WorkManager.ObjectCanReachTarget(unit, mostVulnerableEnemy))
            {
                controller.unit.StartMove(WorkManager.GetTargetClosestPoint(unit, mostVulnerableEnemy));
            }
            else
            {
                controller.unit.StopMove();
            }
        }
    }
}
