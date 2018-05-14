using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class AgroStatus : AiStateStatus
    {
/*
        protected override void OnStatusStart()
        {
            base.OnStatusStart();

            if (target && target.GetStateController())
            {
                target.GetStateController().indicatedObject.StopMove();
            }
        }
*/
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                if (targetStateController)
                {
                    // targetStateController.indicatedObject.StopMove();

                    targetStateController.chaseTarget = inflictor;
                    targetStateController.TransitionToState(ResourceManager.GetAiState("Under Agro"));
                }
            }
        }
    }
}

