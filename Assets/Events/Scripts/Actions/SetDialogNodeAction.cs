using UnityEngine;
using Dialog;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/SetDialogNode")]
    public class SetDialogNodeAction : Action
    {
        public DialogNode dialogNode;

        public override void Act(StateController controller)
        {
            if (dialogNode)
            {
                controller.dialogManager.SetDialogNode(dialogNode);
            }
        }
    }
}