using System.Collections.Generic;
using System.Linq;
using Events;
using Persistence;
using RTS;
using UnityEngine;

public class ArenaRoomTrigger : EventObject {

    public Player enemyPlayer;
    public Flag arenaCenter;
    public float arenaRadius;

    // [Header("Blocking Wall")]
    // public BlockingWallParams blockingWallDefinition;

    public BlockingWall blockingDoor;

    private List<WorldObject> enemyUnits;

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
            if (AreEnemyUnitsDead() && blockingDoor)
            {
                Destroy(blockingDoor.gameObject);
            }

            if (AreEnemyUnitsDead() && IsWallDestroyed())
            {
                inProgress = false;
                completed = true;
            }
        }
    }
    public override void RunEventScript()
    {
        enemyUnits = WorkManager.GetAllyObjects(
           WorkManager.FindNearbyUnits(arenaCenter.transform.position, arenaRadius).Cast<WorldObject>().ToList(),
           enemyPlayer
       )
       .Where(obj => obj is Unit)
       .ToList();

        inProgress = true;
    }

    public override EventObjectData GetData()
    {
        EventObjectData baseData = base.GetData();

        ArenaRoomTriggerData data = new ArenaRoomTriggerData(baseData);
        data.arenaCenter = arenaCenter.transform.position;
        data.arenaRadius = arenaRadius;

        if (blockingDoor)
        {
            data.blockingWall = blockingDoor.GetData();
        }
        else
        {
            data.blockingWall = null;
        }

        return data;
    }

    public override void SetData(EventObjectData data)
    {
        base.SetData(data);

        var arenaRoomTriggerData = ((ArenaRoomTriggerData)data);

        arenaRadius = arenaRoomTriggerData.arenaRadius;

        var flagObj = Instantiate(ResourceManager.GetWorldObject("ArenaRoomFlag"), arenaRoomTriggerData.arenaCenter, transform.rotation);
        arenaCenter = flagObj.GetComponent<Flag>();

        var enemyPlayers = FindObjectsOfType<Player>();

        foreach (var enemyPlayer in enemyPlayers)
        {
            if (!enemyPlayer.human)
            {
                this.enemyPlayer = enemyPlayer;
                enemyPlayer.Start();
                break;
            }
        }

        if (arenaRoomTriggerData.blockingWall != null)
        {
            var blookingDoorBuilding = enemyPlayer.AddBuilding(
                "BlockingWall",
                arenaRoomTriggerData.blockingWall.position,
                arenaRoomTriggerData.blockingWall.rotation,
                arenaRoomTriggerData.blockingWall.objectName
            );
            blockingDoor = blookingDoorBuilding.GetComponent<BlockingWall>();
            blockingDoor.SetData(arenaRoomTriggerData.blockingWall);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(arenaCenter.transform.position, arenaRadius);
    }

    private bool IsWallDestroyed()
    {
        return !blockingDoor;
    }

    private bool AreEnemyUnitsDead()
    {
        enemyUnits = enemyUnits.Where(unit => unit).ToList();
        return enemyUnits.Count() == 0;
    }
}
