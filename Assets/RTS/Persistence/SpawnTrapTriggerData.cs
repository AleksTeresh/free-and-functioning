using System;
using Events;

namespace Persistence
{
    [Serializable]
    public class SpawnTrapTriggerData : EventObjectData
    {
        public BlockingWallParams[] blockingWallDefinitions;

        public SpawnTrapTriggerData(EventObjectData baseData) : base(baseData) { }
    }
}
