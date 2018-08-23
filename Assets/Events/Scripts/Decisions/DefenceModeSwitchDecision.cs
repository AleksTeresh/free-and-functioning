using UnityEngine;
using RTS.Constants;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/DefenceModeSwitch")]
    public class DefenceModeSwitchDecision : Decision
    {
        private bool defenceModeCommandFired;

        void OnEnable()
        {
            EventManager.StartListening(EventNames.SWITCH_HOLD_POSIITON_COMMAND, SetDefenceSwitchFired);
        }

        void OnDisable()
        {
            EventManager.StopListening(EventNames.SWITCH_HOLD_POSIITON_COMMAND, SetDefenceSwitchFired);
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
