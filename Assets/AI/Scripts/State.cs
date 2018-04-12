using System.Collections;
using System.Collections.Generic;
using RTS;
using UnityEngine;

[CreateAssetMenu (menuName ="AI/State")]
public class State : ScriptableObject {

    public Action[] actions;
    public Transition[] transitions;

    public void UpdateState (StateController controller)
    {
        FindNearbyObjects(controller);
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions (StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }

    private void FindNearbyObjects(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, unit.detectionRange);

        controller.nearbyAllies = WorkManager.GetAllyObjects(nearbyObjects, unit.GetPlayer());
        controller.nearbyEnemies = WorkManager.GetEnemyObjects(nearbyObjects, unit.GetPlayer());
    }
}
