using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu(menuName = "AI/Decisions/ShouldRunAway")]
public class ShouldRunAwayDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        Unit self = controller.unit;

        MeleeUnit closestChaser = WorkManager.FindNearestMeleeObject(
            controller.nearbyEnemies
                .Where(p => p && p.GetStateController() && p.GetStateController().chaseTarget &&
                        p.GetStateController().chaseTarget.ObjectId == self.ObjectId)
                .ToList(),
            self.transform.position
        );

        if (!closestChaser)
        {
            return false;
        }

        Vector3 targetLocation = closestChaser.transform.position;
        Vector3 direction = targetLocation - self.transform.position;

        bool shouldRunAway = closestChaser != null && // target exists
            closestChaser.gameObject.activeSelf && // target is alive
            direction.sqrMagnitude < self.weaponRange * self.weaponRange * Constants.RUNAWAY_COEF * Constants.RUNAWAY_COEF && // target is close enough to start running away
            closestChaser.GetNavMeshAgent().speed <= controller.navMeshAgent.speed - Constants.RUNAWAY_SPEED_THREASHOLD;

        return shouldRunAway;
    }
}
