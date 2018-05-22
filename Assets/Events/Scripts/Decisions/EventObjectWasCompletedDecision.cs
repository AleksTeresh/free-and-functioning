using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/EventObjectWasCompleted")]
    public class EventObjectWasCompletedDecision : Decision
    {
        public string eventObjectName;

        public override bool Decide(StateController controller)
        {
            return controller.IsEventObjectCompleted(eventObjectName);
        }
    }
}
