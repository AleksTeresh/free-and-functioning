using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class AgroStatus : Status
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                if (targetStateController)
                {
                    targetStateController.unit.StopMove();

                    targetStateController.chaseTarget = inflicter;
                    targetStateController.TransitionToState(ResourceManager.GetAiState("Chase Idler"));
                }
            }
        }
    }
}

