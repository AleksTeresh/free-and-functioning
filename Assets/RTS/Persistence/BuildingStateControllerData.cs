using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class BuildingStateControllerData : StateControllerData
    {
        public float spawnTimer;

        public int controlledBuildingId;

        public BuildingStateControllerData (
            StateControllerData baseStateController,
            float spawnTimer,
            int controlledBuildingId
        ) : base(baseStateController)
        {
            this.spawnTimer = spawnTimer;
            this.controlledBuildingId = controlledBuildingId;
        }
    }
}
