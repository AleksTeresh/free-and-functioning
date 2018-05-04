﻿using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/DefenceModeSwitch")]
    public class DefenceModeSwitchDecision : Decision
    {
        private bool defenceModeCommandFired;

        void OnEnable()
        {
            EventManager.StartListening("SwitchHoldPositionCommand", SetDefenceSwitchFired);
        }

        void OnDisable()
        {
            EventManager.StopListening("SwitchHoldPositionCommand", SetDefenceSwitchFired);
        }

        public override bool Decide(StateController controller)
        {
            bool defenceModeCommandFired = this.defenceModeCommandFired;
            // reset the variable
            this.defenceModeCommandFired = false;

            return defenceModeCommandFired;

        }

        private void SetDefenceSwitchFired()
        {
            defenceModeCommandFired = true;
        }
    }
}