using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class BlockingWallParams
    {
        public Vector3 wallPosition;
        public Quaternion wallRotation;
        public string name;
    }
}
