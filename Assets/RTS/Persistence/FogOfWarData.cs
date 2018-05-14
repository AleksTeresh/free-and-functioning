using System;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class FogOfWarData
    {
        // public Texture2D shadowMap;
        public Color32[] pixels;
        public int interpolateStartFrame;
        // public Texture2D lastShadowMap;
    }
}
