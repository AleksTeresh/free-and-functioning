using System;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class EventObjectData
    {
        public string type;
        public int objectId;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public bool triggerred;
        public bool inProgress;
        public bool completed;

        public EventObjectData() { }

        public EventObjectData(EventObjectData baseData)
        {
            type = baseData.type;
            objectId = baseData.objectId;
            position = baseData.position;
            rotation = baseData.rotation;
            scale = baseData.scale;
            triggerred = baseData.triggerred;
            inProgress = baseData.inProgress;
            completed = baseData.completed;
        }
    }
}
