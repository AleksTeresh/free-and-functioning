using Statuses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class WorldObjectData
    {
        public string type;
        public int objectId;
        public string objectName;
        public int hitPoints;
        public bool isBusy;
        public List<StatusData> activeStatuse;
        public bool currentlySelected;
        public bool movingIntoPosition;
        public bool aiming;
        public Quaternion aimRotation;
        public int attackDelayFrameCounter;
        public float currentWeaponChargeTime;
        public float currentWeaponMultiChargeTime;
        public float currentAttackDelayTime;
        public bool isInvincible;
        public StateControllerData stateController;
        public int underAttackFrameCounter;
        public FogOfWarAgentData fogOfWarAgent;

        public Vector3 position;
        public Quaternion rotation;

        public WorldObjectData (
            string type,
            int objectId,
            string objectName,
            int hitPoints,
            bool isBusy,
            List<StatusData> activeStatuse,
            bool currentlySelected,
            bool movingIntoPosition,
            bool aiming,
            Quaternion aimRotation,
            int attackDelayFrameCounter,
            float currentWeaponChargeTime,
            float currentWeaponMultiChargeTime,
            float currentAttackDelayTime,
            bool isInvincible,
            StateControllerData stateController,
            int underAttackFrameCounter,
            FogOfWarAgentData fogOfWarAgent,
            Vector3 position,
            Quaternion rotation
        )
        {
            this.type = type;
            this.objectId = objectId;
            this.objectName = objectName;
            this.hitPoints = hitPoints;
            this.isBusy = isBusy;
            this.activeStatuse = activeStatuse;
            this.currentlySelected = currentlySelected;
            this.movingIntoPosition = movingIntoPosition;
            this.aiming = aiming;
            this.aimRotation = aimRotation;
            this.attackDelayFrameCounter = attackDelayFrameCounter;
            this.currentWeaponChargeTime = currentWeaponChargeTime;
            this.currentWeaponMultiChargeTime = currentWeaponMultiChargeTime;
            this.currentAttackDelayTime = currentAttackDelayTime;
            this.isInvincible = isInvincible;
            this.stateController = stateController;
            this.underAttackFrameCounter = underAttackFrameCounter;
            this.fogOfWarAgent = fogOfWarAgent;
            this.position = position;
            this.rotation = rotation;
        }

        public WorldObjectData (
            WorldObjectData baseData
        )
        {
            this.type = baseData.type;
            this.objectId = baseData.objectId;
            this.objectName = baseData.objectName;
            this.hitPoints = baseData.hitPoints;
            this.isBusy = baseData.isBusy;
            this.activeStatuse = baseData.activeStatuse;
            this.currentlySelected = baseData.currentlySelected;
            this.movingIntoPosition = baseData.movingIntoPosition;
            this.aiming = baseData.aiming;
            this.aimRotation = baseData.aimRotation;
            this.attackDelayFrameCounter = baseData.attackDelayFrameCounter;
            this.currentWeaponChargeTime = baseData.currentWeaponChargeTime;
            this.currentWeaponMultiChargeTime = baseData.currentWeaponMultiChargeTime;
            this.currentAttackDelayTime = baseData.currentAttackDelayTime;
            this.isInvincible = baseData.isInvincible;
            this.stateController = baseData.stateController;
            this.underAttackFrameCounter = baseData.underAttackFrameCounter;
            this.fogOfWarAgent = baseData.fogOfWarAgent;
            this.position = baseData.position;
            this.rotation = baseData.rotation;
        }
    }
}
