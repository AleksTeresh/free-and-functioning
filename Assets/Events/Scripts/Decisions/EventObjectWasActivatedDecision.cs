using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/EventObjectWasActivated")]
    public class EventObjectWasActivatedDecision : Decision
    {
        public string eventObjectName;

        public override bool Decide(StateController controller)
        {
            return controller.IsEventObjectActivated(eventObjectName);
        }
    }
}
