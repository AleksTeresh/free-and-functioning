using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class GameData
    {
        public PlayerData[] players;
        public CameraData mainCamera;
        public SunData sun;
        public ExtraLightData[] extraLights;
        public EventObjectData[] eventObjects;
        public FogOfWarData fogOfWar;
        public TargetManagerData targetManager;

        public string sceneName;
    }
}
