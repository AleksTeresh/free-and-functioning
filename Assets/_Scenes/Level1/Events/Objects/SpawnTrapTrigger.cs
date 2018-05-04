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

    [Header("Blocking Wall")]
    public Vector3 wallPosition = new Vector3(483.379f, 10.70251f, 568.8833f);
    public Quaternion wallRotation = new Quaternion(0, 0, 0, 1);

    private bool triggerred = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggerred)
        {
            triggerred = true;

            var spawnPoints = WorkManager.FindNearbyObjects<SpawnPoint>(transform.position, spawnPointDetectRange)
                .ToList();

            // TODO: the ardcoded blocking wall, should be definied by parameters instead
            enemyPlayer.AddBuilding("BlockingWall", wallPosition, wallRotation);

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
