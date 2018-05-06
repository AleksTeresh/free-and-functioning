using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Actions/ResetTarget")]
    public class ResetTargetAction : Action
    {
        // private static readonly float WALK_RADIUS = 10;

        public override void Act(StateController controller)
        {
            ResetTarget(controller);
        }

        private void ResetTarget(StateController controller)
        {
            controller.chaseTarget = null;
        }
    }
}
