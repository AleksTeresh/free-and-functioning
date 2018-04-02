using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class StunStatus : Status
    {
        private string previousState;

        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                // if the target just received the status, save its current state
                if (previousState == null)
                {
                    previousState = targetStateController.currentState.name;
                }
                

                // targetStateController.chaseTarget = null;
                targetStateController.TransitionToState(ResourceManager.GetAiState("Stunned"));
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
