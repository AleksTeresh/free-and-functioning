using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

namespace Persistence
{
    [Serializable]
    public class UnitStateControllerData : StateControllerData
    {
        public List<Transform> wayPointList;
        public int nextWayPoint;
        public int allyAbilityTargetId;
        public int enemyAbilityTargetId;
        public int controlledUnitId;
        public Vector3 aoeAbilityTarget;
        public AbilityData abilityToUse;

        public UnitStateControllerData (
            StateControllerData baseStateController,
            List<Transform> wayPointList,
            int nextWayPoint,
            int allyAbilityTargetId,
            int enemyAbilityTargetId,
            int controlledUnitId,
            Vector3 aoeAbilityTarget,
            AbilityData abilityToUse
        ) : base(baseStateController)
        {
            this.wayPointList = wayPointList;
            this.nextWayPoint = nextWayPoint;
            this.allyAbilityTargetId = allyAbilityTargetId;
            this.enemyAbilityTargetId = enemyAbilityTargetId;
            this.controlledUnitId = controlledUnitId;
            this.aoeAbilityTarget = aoeAbilityTarget;
            this.abilityToUse = abilityToUse;
        }
    }
}
