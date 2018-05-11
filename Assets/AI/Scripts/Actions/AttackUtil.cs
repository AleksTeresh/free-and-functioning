using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace AI
{
    public static class AttackUtil
    {
        public static void HandleSingleModeAttack(StateController controller)
        {
            WorldObject controlledObject = controller.controlledObject;
            WorldObject chaseTarget = controller.chaseTarget;

            // if no target or canot attack, return
            if (chaseTarget == null || !controlledObject.CanAttack())
            {
                return;
            }

            Vector3 currentPosition = controlledObject.transform.position;
            Vector3 currentEnemyPosition = WorkManager.GetTargetClosestPoint(controlledObject, chaseTarget);
            Vector3 direction = currentEnemyPosition - currentPosition;

            if (
                direction.sqrMagnitude < controlledObject.weaponRange * controlledObject.weaponRange
            )
            {
                controller.attacking = true;
                controlledObject.PerformAttack(chaseTarget);
            }
            else
            {
                controller.attacking = false;
            }
        }

        public static WorldObject SetClosestEnemyAsTarget (StateController controller)
        {
            Vector3 currentPosition = controller.controlledObject.transform.position;

            WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

            if (!closestEnemy && controller.targetManager && controller.targetManager.SingleTarget)
            {
                closestEnemy = controller.targetManager.SingleTarget;
            }

            if (closestEnemy)
            {
                controller.chaseTarget = closestEnemy;
            }

            return closestEnemy;
        }
    }
}
