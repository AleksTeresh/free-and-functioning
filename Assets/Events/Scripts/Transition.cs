using System;

namespace Events
{
    [Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        // public State falseState;
    }
}
