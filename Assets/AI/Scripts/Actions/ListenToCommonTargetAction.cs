using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/ListenToCommonTarget")]
    public class ListenToCommonTargetAction : Action
    {
        public override void Act(StateController controller)
        {
            if (controller.targetManager && controller.targetManager.SingleTarget)
            {
                controller.chaseTarget = controller.targetManager.SingleTarget;
            }
        }
    }
}

