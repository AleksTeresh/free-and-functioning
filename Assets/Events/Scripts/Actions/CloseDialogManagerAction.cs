using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/CloseDialogManager")]
    public class CloseDialogManagerAction : Action
    {
        public override void Act(StateController controller)
        {
            controller.dialogManager.EndDialog();
        }
    }
}
