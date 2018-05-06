using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/ChangeTargetToClosest")]
    public class ChangeTargetToClosestAction : Action
    {
        public override void Act(StateController controller)
        {
            WorldObject controlledObject = controller.controlledObject;
            Vector3 currentPosition = controlledObject.transform.position;
            WorldObject chaseTarget = controller.chaseTarget;

            WorldObject closestEnemy = WorkManager.FindNearestWorldObjectInListToPosition(controller.nearbyEnemies, currentPosition);

            if (closestEnemy)
            {
                controller.chaseTarget = closestEnemy;

                if (!(controlledObject is Unit))
                {
                    return;
                }

                Unit unit = (Unit)controlledObject;
                if (!WorkManager.ObjectCanReachTarget(controlledObject, closestEnemy))
                {

                    unit.StartMove(WorkManager.GetTargetClosestPoint(unit, closestEnemy));
                }
                else
                {
                    unit.StopMove();
                }
            }
        }
    }
}
