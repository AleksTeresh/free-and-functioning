using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class ConfuseStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                targetStateController.TransitionToState(ResourceManager.GetAiState("Confused"));
            }
        }
    }
}

