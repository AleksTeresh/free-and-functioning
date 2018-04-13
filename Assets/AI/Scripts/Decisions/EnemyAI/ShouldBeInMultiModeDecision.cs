using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using AI;

[CreateAssetMenu(menuName = "AI/Decisions/EnemyAI/ShouldBeInMultiMode")]
public class ShouldBeInMultiModeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var unit = controller.unit;

        if (controller.nearbyEnemies.Count <= 3) return false;

        var nearestMeleeEnemy = WorkManager.FindNearestMeleeObject(controller.nearbyEnemies, controller.transform.position);

        return !nearestMeleeEnemy || (nearestMeleeEnemy.transform.position - controller.transform.position).sqrMagnitude > unit.weaponRange * Constants.RUNAWAY_COEF * unit.weaponRange * Constants.RUNAWAY_COEF;
    }
}
