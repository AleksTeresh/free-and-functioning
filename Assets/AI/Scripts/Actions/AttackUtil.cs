using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    public static class AttackUtil
    {
        public static void HandleSingleModeAttack(StateController controller)
        {
            Unit unit = controller.unit;
            WorldObject chaseTarget = controller.chaseTarget;

            // if no target or canot attack, return
            if (chaseTarget == null || !unit.CanAttack())
            {
                return;
            }

            Vector3 currentPosition = unit.transform.position;
            Vector3 currentEnemyPosition = chaseTarget.transform.position;
            Vector3 direction = currentEnemyPosition - currentPosition;

            if (
                direction.sqrMagnitude < unit.weaponRange * unit.weaponRange
            )
            {
                controller.attacking = true;
                controller.unit.PerformAttack(chaseTarget);
            }
            else
            {
                controller.attacking = false;
            }
        }
    }
}
