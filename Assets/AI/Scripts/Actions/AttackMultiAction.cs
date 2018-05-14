using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/AttackMulti")]
    public class AttackMultiAction : Action
    {
        public override void Act(StateController controller)
        {
            AttackMulti(controller);
        }

        private void AttackMulti(StateController controller)
        {
            var controlledObject = controller.controlledObject;

            // if cannot attack multi, fallback to single attack
            if (!controlledObject.CanAttackMulti())
            {
                AttackUtil.SetClosestEnemyAsTarget(controller);

                AttackUtil.HandleSingleModeAttack(controller);
                return;
            }

            Vector3 currentPosition = controlledObject.transform.position;
            List<WorldObject> reachableEnemies = controller.nearbyEnemies
                .Where(p =>
                {
                    Vector3 currentEnemyPosition = WorkManager.GetTargetClosestPoint(controlledObject, p);
                    Vector3 direction = currentEnemyPosition - currentPosition;

                    return direction.sqrMagnitude < controlledObject.weaponRange * controlledObject.weaponRange;
                })
                .ToList();

            controller.attacking = reachableEnemies.Count > 0;
            controlledObject.PerformAttackToMulti(reachableEnemies);
        }
    }
}
