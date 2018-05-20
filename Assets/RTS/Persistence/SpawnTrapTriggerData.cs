using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class SpawnTrapTriggerData : EventObjectData
    {
        public SpawnTrapTrigger.BlockingWallParams[] blockingWallDefinitions;

        public SpawnTrapTriggerData(EventObjectData baseData) : base(baseData) { }
    }
}
