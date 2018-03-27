using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public static class InputToCommandManager
    {
        public static void ToChaseState(StateController stateController, WorldObject chaseTarget)
        {
            // set clicked object as a target
            stateController.chaseTarget = chaseTarget;
            // transition to Chase Manual State
            stateController.TransitionToState(ResourceManager.GetAiState("Chase Manual"));
        }

        public static void ToBusyState(StateController stateController, Vector3 destination)
        {
            // remove current target if present
            stateController.chaseTarget = null;
            // add new destination for nav mesh agent
            stateController.navMeshAgent.SetDestination(destination);
            // transition to Chase Manual State
            stateController.TransitionToState(ResourceManager.GetAiState("Busy Manual"));
        }
    }
}

