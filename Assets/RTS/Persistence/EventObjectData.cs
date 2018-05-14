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
        public bool triggerred;
    }
}
