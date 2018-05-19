﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class StateControllerData
    {
        public string currentState;
        public string defaultState;
        public int chaseTargetId;
        public int controlledObjectId;
        public bool attacking;
        public bool aiActive;

        public StateControllerData (
            string currentState,
            string defaultState,
            int chaseTargetId,
            int controlledObjectId,
            bool attacking,
            bool aiActive
        )
        {
            this.currentState = currentState;
            this.defaultState = defaultState;
            this.chaseTargetId = chaseTargetId;
            this.controlledObjectId = controlledObjectId;
            this.attacking = attacking;
            this.aiActive = aiActive;
        }

        public StateControllerData (
            StateControllerData baseData
        )
        {
            this.currentState = baseData.currentState;
            this.defaultState = baseData.defaultState;
            this.chaseTargetId = baseData.chaseTargetId;
            this.controlledObjectId = baseData.controlledObjectId;
            this.attacking = baseData.attacking;
            this.aiActive = baseData.aiActive;
        }
    }
}