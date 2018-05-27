using System.Linq;
using UnityEngine;
using RTS;
using System;
using Events;
using Persistence;
using System.Collections.Generic;

public class SpawnTrapTrigger : EventObject
{
    public float spawnPointDetectRange = 100;
    public Player enemyPlayer;
    public Unit unit;

    [Header("Blocking Wall")]
    public BlockingWallParams[] blockingWallDefinitions;

    private List<Unit> eventEnemyUnits;
    private Building frontBlockingDoor;
    private Building backBlockingDoor;

    public SpawnTrapTrigger()
    {
        eventEnemyUnits = new List<Unit>(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggerred)
        {
            triggerred = true;
        }
    }


    private void Update()
    {
        if (inProgress)
        {
            if (AreEnemyUnitsDead() && frontBlockingDoor)
            {
                Destroy(frontBlockingDoor.gameObject);
            }

            if (IsFrontWallDestroyed())
            {
                DestroyAllEventEnemies();
            }

            if (AreEnemyUnitsDead() && IsFrontWallDestroyed())
            {
                inProgress = false;
                completed = true;
            }
        }
    }

    private void DestroyAllEventEnemies()
    {
        foreach (Unit unit in eventEnemyUnits)
        {
            if (unit)
            {
                Destroy(unit.gameObject);
            }
        }
    }

    private bool AreEnemyUnitsDead()
    {
        foreach (Unit unit in eventEnemyUnits)
        {
            if (unit)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFrontWallDestroyed()
    {
        return !frontBlockingDoor;
    }

    public override void RunEventScript()
    {
        var spawnPoints = WorkManager.FindNearbyObjects<SpawnPoint>(transform.position, spawnPointDetectRange).ToList();

        foreach (var blockingWallParams in blockingWallDefinitions)
        {
            Building blockingWall = enemyPlayer.AddBuilding(
                "BlockingWall",
                blockingWallParams.wallPosition,
                blockingWallParams.wallRotation,
                blockingWallParams.name
            );

            if (blockingWallParams.name == "Back Wall")
            {
                backBlockingDoor = blockingWall;
            }

            if (blockingWallParams.name == "Front Wall")
            {
                frontBlockingDoor = blockingWall;
            }
        }

        spawnPoints.ForEach(p =>
        {
            Unit eventEnemyUnit = enemyPlayer.AddUnit(unit.name, p.transform.position, p.transform.position, p.transform.rotation, null);

            eventEnemyUnits.Add(eventEnemyUnit);
        });

        inProgress = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnPointDetectRange);
    }

    public override EventObjectData GetData()
    {
        EventObjectData baseData = base.GetData();

        SpawnTrapTriggerData data = new SpawnTrapTriggerData(baseData);
        data.blockingWallDefinitions = blockingWallDefinitions;

        return data;
    }

    public override void SetData(EventObjectData data)
    {
        base.SetData(data);

        blockingWallDefinitions = ((SpawnTrapTriggerData)data).blockingWallDefinitions;

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
}
