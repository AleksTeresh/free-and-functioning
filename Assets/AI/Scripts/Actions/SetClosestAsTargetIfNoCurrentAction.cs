using RTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/SetClosestAsTargetIfNoCurrent")]
    public class SetClosestAsTargetIfNoCurrentAction : Action
    {
        public override void Act (StateController controller)
        {
            WorldObject controlledObject = controller.controlledObject;
            Vector3 currentPosition = controlledObject.transform.position;

            WorldObject commonTarget = controller.targetManager.SingleTarget;

            if (!commonTarget)
            {
                WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

                if (closestEnemy)
                {
                    controller.targetManager.SingleTarget = closestEnemy;
                }
            }
        }
    }
}
