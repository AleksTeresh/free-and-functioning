using System;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class ExtraLightData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float scrollSpeed;
        public float scrollDistance;
        public Vector3 scrollDirection;
        public Vector3 initialPosition;

        public Light tileLight;
        public Light tileLight2;
        public Light gridLight;
    }
}
