using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/SwitchEnemyCommandWasFired")]
    public class SwitchEnemyCommandWasFiredDecision : Decision
    {
        private bool switchEnemyCommandFired;

        void OnEnable()
        {
            EventManager.StartListening("SwitchEnemyCommand", SetSwitchEnemyCommandFired);
        }

        void OnDisable()
        {
            EventManager.StopListening("SwitchEnemyCommand", SetSwitchEnemyCommandFired);
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
