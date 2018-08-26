using UnityEngine;
using RTS.Constants;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/SwitchEnemyCommandWasFired")]
    public class SwitchEnemyCommandWasFiredDecision : Decision
    {
        private bool switchEnemyCommandFired;

        void OnEnable()
        {
            EventManager.StartListening(EventNames.SWITCH_ENEMY_COMMAND, SetSwitchEnemyCommandFired);
        }

        void OnDisable()
        {
            EventManager.StopListening(EventNames.SWITCH_ENEMY_COMMAND, SetSwitchEnemyCommandFired);
        }

        public override bool Decide(StateController controller)
        {
            bool switchEnemyCommandFired = this.switchEnemyCommandFired;
            // reset the variable
            this.switchEnemyCommandFired = false;

            return switchEnemyCommandFired;

        }

        private void SetSwitchEnemyCommandFired()
        {
            switchEnemyCommandFired = true;
        }
    }
}
