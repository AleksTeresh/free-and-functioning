using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/ChangeTargetToMostVulnerable")]
    public class ChangeTargetToMostVulnerableAction : Action
    {
        public override void Act(StateController controller)
        {
            WorldObject controlledObject = controller.controlledObject;
            Vector3 currentPosition = controlledObject.transform.position;
            WorldObject chaseTarget = controller.chaseTarget;

            WorldObject mostVulnerableEnemy = WorkManager.FindMostVulnerableObjectInList(controller.nearbyEnemies);

            if (mostVulnerableEnemy)
            {
                controller.chaseTarget = mostVulnerableEnemy;

                if (!(controlledObject is Unit)) return;
                Unit unit = (Unit)controlledObject;

                if (!WorkManager.ObjectCanReachTarget(unit, mostVulnerableEnemy))
                {
                    unit.StartMove(WorkManager.GetTargetClosestPoint(unit, mostVulnerableEnemy));
                }
                else
                {
                    unit.StopMove();
                }
            }
        }
    }

}
