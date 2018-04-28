using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/AttackIsMulti")]
    public class AttackIsMultiDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.targetManager &&
                controller.targetManager.InMultiMode;
        }
    }
}
