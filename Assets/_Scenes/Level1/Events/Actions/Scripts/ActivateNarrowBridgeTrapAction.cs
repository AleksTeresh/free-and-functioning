using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/Level1/ActivateNarrowBridgeTrapAction")]
    public class ActivateNarrowBridgeTrapAction : Action
    {
        public string trapTriggerName;

        public override void Act(StateController controller)
        {
            EventObject trapTrigger = controller.GetEventObjectByName(trapTriggerName);

            if (!trapTrigger)
            {
                Debug.LogWarningFormat("Narrow bridge trigger with name {0} was not registered", trapTriggerName);
            } else
            {
                trapTrigger.RunEventScript();
            }
        }
    }
}
