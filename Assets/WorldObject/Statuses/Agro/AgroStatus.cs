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
                target.GetStateController().unit.StopMove();
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
                    // targetStateController.unit.StopMove();

                    targetStateController.chaseTarget = inflicter;
                    targetStateController.TransitionToState(ResourceManager.GetAiState("Under Agro"));
                }
            }
        }
    }
}

