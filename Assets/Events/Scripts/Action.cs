using UnityEngine;

namespace Events
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}
