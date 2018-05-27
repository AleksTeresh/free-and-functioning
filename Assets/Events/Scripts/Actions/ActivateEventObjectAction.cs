using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/ActivateEventObject")]
    public class ActivateEventObjectAction : Action
    {
        public string eventObjectName;

        public override void Act(StateController controller)
        {
            EventObject eventObject = controller.GetEventObjectByName(eventObjectName);

            if (!eventObject)
            {
                Debug.LogWarningFormat("Event object with name {0} was not registered", eventObjectName);
            }
            else
            {
                eventObject.RunEventScript();
            }
        }
    }
}

