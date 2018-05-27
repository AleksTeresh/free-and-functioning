using System;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class ArenaRoomTriggerData : EventObjectData
    {
        public BuildingData blockingWall;
        public Vector3 arenaCenter;
        public float arenaRadius;

        public ArenaRoomTriggerData(EventObjectData baseData) : base(baseData) { }
    }
}
