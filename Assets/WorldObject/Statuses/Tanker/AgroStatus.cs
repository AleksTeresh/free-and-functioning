using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class AgroStatus : Status
    {
        private WorldObject agroObject;

        protected override void AffectTarget()
        {
            var targetStateController = target.GetStateController();

            targetStateController.chaseTarget = inflicter;
            targetStateController.TransitionToState(ResourceManager.GetAiState("Chase Idler"));
        }
    }
}

