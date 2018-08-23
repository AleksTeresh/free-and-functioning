using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using RTS.Constants;

namespace Statuses
{
    public class DisarmementStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                targetStateController.TransitionToState(ResourceManager.GetAiState(AIStates.DISARMED));
            }
        }
    }
}
