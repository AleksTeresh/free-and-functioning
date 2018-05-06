using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/SetAIStateToObjectsNearby")]
    public class SetAIStateToObjectsNearbyAction : Action
    {
        public Flag center;
        public WorldObject prefabByName;
        public float radius;
        public AI.State state;

        public override void Act(StateController controller)
        {
            if (state)
            {
                var center = new List<Flag>(FindObjectsOfType<Flag>()).Find(p => p.name == this.center.name);
                var nearbyObjects = WorkManager.FindNearbyObjects(center.transform.position, radius);

                nearbyObjects.ForEach(obj =>
                {
                    if (obj.name == prefabByName.name)
                    {
                        var stateController = obj.GetStateController();

                        if (stateController && stateController.currentState.name != state.name)
                        {
                            stateController.TransitionToState(state);
                        }
                    }
                });
            }
        }
    }
}