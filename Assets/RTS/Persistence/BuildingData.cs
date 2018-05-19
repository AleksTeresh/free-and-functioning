using System;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class BuildingData : WorldObjectData
    {
        public BuildingStateControllerData buildingStateController;
        public Queue<string> buildQueue;
        public Vector3 rallyPoint;
        public float currentBuildProgress;
        public Vector3 spawnPoint;

        public BuildingData(
            WorldObjectData baseObjectData,
            BuildingStateControllerData stateController,
            Queue<string> buildQueue,
            Vector3 rallyPoint,
            float currentBuildProgress,
            Vector3 spawnPoint
        ) : base(baseObjectData)
        {
            this.stateController = null;
            this.buildingStateController = stateController;
            this.buildQueue = buildQueue;
            this.rallyPoint = rallyPoint;
            this.currentBuildProgress = currentBuildProgress;
            this.spawnPoint = spawnPoint;
        }
    }
}
