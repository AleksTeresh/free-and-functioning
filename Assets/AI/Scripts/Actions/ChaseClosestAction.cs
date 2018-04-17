﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu(menuName = "AI/Actions/ChaseClosest")]
public class ChaseClosestAction : UnitAction
{

    protected override void DoAction(UnitStateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;

        WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

        if (closestEnemy && !unit.holdingPosition && !WorkManager.ObjectCanReachTarget(unit, closestEnemy))
        {
            controller.chaseTarget = closestEnemy;
            controller.unit.StartMove(WorkManager.GetTargetClosestPoint(unit, closestEnemy));
        }
        else
        {
            controller.unit.StopMove();
        }
    }
}
