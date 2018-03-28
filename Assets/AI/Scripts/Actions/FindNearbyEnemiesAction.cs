using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/FindNearbyEnemies")]
public class FindNearbyEnemiesAction : Action
{

    public override void Act(StateController controller)
    {
        FindNearbyEnemies(controller);
    }

    private void FindNearbyEnemies(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, unit.detectionRange);

        List<WorldObject> enemyObjects = nearbyObjects
            .Where(p => p.GetPlayer() != null && p.GetPlayer() != unit.GetPlayer()) // do not attack friendly units or neutral objects
            .ToList();

        controller.nearbyEnemies = enemyObjects;
    }
}
