using System.Linq;
using UnityEngine;
using RTS;
using System;
using Events;
using Persistence;

public class SpawnTrapTrigger : EventObject
{
    public float spawnPointDetectRange = 100;
    public Player enemyPlayer;
    public Unit unit;

    [Header("Blocking Wall")]
    public BlockingWallParams[] blockingWalls;

    void OnTriggerEnter(Collider other)
    {
        if (!triggerred)
        {
            triggerred = true;

            var spawnPoints = WorkManager.FindNearbyObjects<SpawnPoint>(transform.position, spawnPointDetectRange)
                .ToList();

            foreach (var blockingWall in blockingWalls)
            {
                enemyPlayer.AddBuilding(
                    "BlockingWall",
                    blockingWall.wallPosition,
                    blockingWall.wallRotation,
                    blockingWall.name
                );
            }

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

    public override void SetData(EventObjectData data)
    {
        base.SetData(data);

        var enemyPlayers = FindObjectsOfType<Player>();

        foreach (var enemyPlayer in enemyPlayers)
        {
            if (!enemyPlayer.human)
            {
                this.enemyPlayer = enemyPlayer;
                return;
            }
        }
    }

    [Serializable]
    public class BlockingWallParams
    {
        public Vector3 wallPosition;
        public Quaternion wallRotation;
        public string name;
    }
}
