using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/NextDialogSentence")]
    public class NextDialogSentenceAction : Action
    {
        public override void Act(StateController controller)
        {
            controller.dialogManager.DisplayNextSentence();
        }
    }
}
