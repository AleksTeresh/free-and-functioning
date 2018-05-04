using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;

public class SpawnTrapTrigger : MonoBehaviour
{
    public float spawnPointDetectRange = 100;
    public Player enemyPlayer;
    public Unit unit;

    private bool triggerred = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggerred)
        {
            triggerred = true;

            var spawnPoints = WorkManager.FindNearbyObjects<SpawnPoint>(transform.position, spawnPointDetectRange)
                .ToList();

            spawnPoints.ForEach(p =>
            {
                enemyPlayer.AddUnit(unit.name, p.transform.position, p.transform.position, p.transform.rotation, null);
            });
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnPointDetectRange);
    }
}
