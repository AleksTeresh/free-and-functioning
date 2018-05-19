using System;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class CameraData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public CameraData(Camera camera)
        {
            position = camera.transform.position;
            rotation = camera.transform.rotation;
            scale = camera.transform.localScale;
        }
    }
}
