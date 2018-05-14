using System;
using UnityEngine.AI;
using UnityEngine;
using System.Runtime.Serialization;

namespace Persistence
{
    [Serializable]
    public class UnitData : WorldObjectData
    {
        public UnitStateControllerData unitStateController;
        public AbilityAgentData abilityAgent;
        public bool holdingPosition;
        public Vector3 destination;
        public int aimTargetId;

        public UnitData (
            WorldObjectData baseObjectData,
            UnitStateControllerData stateController,
            AbilityAgentData abilityAgent,
            bool holdingPosition,
            int aimTargetId,
            Vector3 destination
        ) : base(baseObjectData)
        {
            this.stateController = null;
            this.unitStateController = stateController;
            this.abilityAgent = abilityAgent;
            this.holdingPosition = holdingPosition;
            this.aimTargetId = aimTargetId;
            this.destination = destination;
        }

        public UnitData (
            UnitData objectData
        ) : base (
            objectData.type,
            objectData.objectId,
            objectData.objectName,
            objectData.hitPoints,
            objectData.isBusy,
            objectData.activeStatuse,
            objectData.currentlySelected,
            objectData.movingIntoPosition,
            objectData.aiming,
            objectData.aimRotation,
            objectData.attackDelayFrameCounter,
            objectData.currentWeaponChargeTime,
            objectData.currentWeaponMultiChargeTime,
            objectData.currentAttackDelayTime,
            objectData.isInvincible,
            objectData.stateController,
            objectData.underAttackFrameCounter,
            objectData.fogOfWarAgent,
            objectData.position,
            objectData.rotation
        )
        {
            this.stateController = null;
            this.unitStateController = objectData.unitStateController;
            this.abilityAgent = objectData.abilityAgent;
            this.holdingPosition = objectData.holdingPosition;
            this.aimTargetId = objectData.aimTargetId;
            this.destination = objectData.destination;
        }
    }
}
