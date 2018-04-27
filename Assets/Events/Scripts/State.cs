using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/State")]
    public class State : ScriptableObject
    {
        public Action[] enterActions;
        public Action[] updateActions;
        public Action[] exitActions;
        public Transition[] transitions;

        public void EnterState (StateController controller)
        {
            DoEnterActions(controller);
        }

        public void ExitState (StateController controller)
        {
            DoExitActions(controller);
        }

        public void UpdateState(StateController controller)
        {
            DoUpdateActions(controller);
            CheckTransitions(controller);
        }

        private void CheckTransitions(StateController controller)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    controller.TransitionToState(transitions[i].trueState);
                }
            }
        }

        private void DoUpdateActions(StateController controller)
        {
            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Act(controller);
            }
        }

        private void DoExitActions(StateController controller)
        {
            for (int i = 0; i < exitActions.Length; i++)
            {
                exitActions[i].Act(controller);
            }
        }

        private void DoEnterActions(StateController controller)
        {
            for (int i = 0; i < enterActions.Length; i++)
            {
                enterActions[i].Act(controller);
            }
        }
    }
}
