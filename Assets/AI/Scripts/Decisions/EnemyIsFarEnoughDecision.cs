﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Decisions/EnemyIsFarEnough")]
public class EnemyIsFarEnoughDecision : Decision
{
    private static readonly float STOP_RUNNING_COEF = 0.8f;

    public override bool Decide(StateController controller)
    {
        Unit self = controller.unit;
        WorldObject closestChaser = WorkManager.FindNearestMeleeObject(
            controller.nearbyEnemies
                .Where(p => p && p.GetStateController() && p.GetStateController().chaseTarget &&
                        p.GetStateController().chaseTarget.ObjectId == self.ObjectId)
                .ToList(),
            self.transform.position
        );

        // stop running away, if there is no chaser anymore
        if (!closestChaser)
        {
            return true;
        }

        Vector3 targetLocation = closestChaser.transform.position;
        Vector3 direction = targetLocation - self.transform.position;

        bool targetIsFarEnough = closestChaser != null && // target exists
            closestChaser.gameObject.activeSelf && // target is alive
            closestChaser is MeleeUnit && // target is a MeleeUnit,so it makes sense to kite it
            direction.sqrMagnitude > self.weaponRange * self.weaponRange * STOP_RUNNING_COEF * STOP_RUNNING_COEF; // target is far enough to stop running away

        return targetIsFarEnough;
    }
}