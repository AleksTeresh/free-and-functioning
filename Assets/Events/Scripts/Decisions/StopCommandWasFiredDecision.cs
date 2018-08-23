using UnityEngine;
using RTS.Constants;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/StopCommandWasFired")]
    public class StopCommandWasFiredDecision : Decision
    {
        private bool stopCommandFired;

        void OnEnable()
        {
            EventManager.StartListening(EventNames.STOP_COMMAND, SetStopCommandFired);
        }

        void OnDisable()
        {
            EventManager.StopListening(EventNames.STOP_COMMAND, SetStopCommandFired);
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
