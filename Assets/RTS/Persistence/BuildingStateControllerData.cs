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

        public BuildingStateControllerData (
            StateControllerData baseStateController,
            float spawnTimer
        ) : base(baseStateController)
        {
            this.spawnTimer = spawnTimer;
        }
    }
}
