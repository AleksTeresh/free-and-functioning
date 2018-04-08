using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class AiStateStatus : Status
    {
        protected string previousState;

        protected override void OnStatusStart()
        {
            // if the target just received the status, save its current state
            if (previousState == null)
            {
                var targetStateController = target.GetStateController();

                previousState = targetStateController.currentState.name;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target && target.GetStateController() && previousState != null && previousState != "")
            {
                target.GetStateController().TransitionToState(ResourceManager.GetAiState(previousState));
            }
        }
    }

}
