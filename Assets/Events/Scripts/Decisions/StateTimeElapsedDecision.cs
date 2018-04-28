using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/StateTimeElapsed")]
    public class StateTimeElapsedDecision : Decision
    {
        public float requiredElapsedTime = 5;

        public override bool Decide(StateController controller)
        {
            return controller.timeInState >= requiredElapsedTime;
        }
    }
}
