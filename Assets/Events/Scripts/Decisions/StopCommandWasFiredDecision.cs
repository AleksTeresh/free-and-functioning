using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/StopCommandWasFired")]
    public class StopCommandWasFiredDecision : Decision
    {
        private bool stopCommandFired;

        void OnEnable()
        {
            EventManager.StartListening("StopCommand", SetStopCommandFired);
        }

        void OnDisable()
        {
            EventManager.StopListening("StopCommand", SetStopCommandFired);
        }

        public override bool Decide(StateController controller)
        {
            bool stopCommandFired = this.stopCommandFired;
            // reset the variable
            this.stopCommandFired = false;

            return stopCommandFired;

        }

        private void SetStopCommandFired ()
        {
            stopCommandFired = true;
        }
    }
}
