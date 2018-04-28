using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/DialogManagerClosed")]
    public class DialogManagerClosedDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return !controller.dialogManager || !controller.dialogManager.IsActive();
        }
    }
}
