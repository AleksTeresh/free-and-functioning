using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/SubmitWasPressed")]
    public class SubmitWasPressedDecision : Decision
    {
        public override bool Decide (StateController controller)
        {
            return Input.GetButtonDown("Submit");
        }
    }
}
