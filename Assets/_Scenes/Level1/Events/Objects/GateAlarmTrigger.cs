using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;

public class GateAlarmTrigger : MonoBehaviour {
    public float alarmRange = 150;

    private bool triggerred = false;

    public Vector3 relativeRallyPoint;

    void OnTriggerEnter(Collider other)
    {
        if (!triggerred)
        {
            triggerred = true;

            var nearbyEnemyUnits = WorkManager.FindNearbyUnits(transform.position, alarmRange)
                .Where(p => !p.GetPlayer().human)
                .ToList();

            var poistionToMoveTo = WorkManager.GetClosestPointOnNavMesh(
                transform.position + relativeRallyPoint,
                "Walkable",
                20
            );

            if (poistionToMoveTo.HasValue)
            {
                nearbyEnemyUnits.ForEach(unit => {
                    unit.StartMove(poistionToMoveTo.Value);
                });
            }
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alarmRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + relativeRallyPoint, 5);
    }
}
