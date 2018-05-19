using System;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class PlayerData
    {
        public int selectedObjectId;
        public List<int> selectedObjectIds;
        public Color teamColor;

        public string username;
        public bool human;

        public bool lockCursor;

        public List<UnitData> units;
        public List<BuildingData> buildings;

        public EventStateControllerData eventStateController;
    }
}
