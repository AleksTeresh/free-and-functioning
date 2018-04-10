using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Statuses
{
    public class SilenceStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target is Unit)
            {
                var targetUnit = (Unit)target;

                ResetAbilitiesTimers(targetUnit);
            }
        }

        private void ResetAbilitiesTimers(Unit targetUnit)
        {
            foreach (var ability in targetUnit.abilities)
            {
                ability.cooldownTimer = 0.0f;
            }

            foreach (var ability in targetUnit.abilitiesMulti)
            {
                ability.cooldownTimer = 0.0f;
            }
        }
    }
}
