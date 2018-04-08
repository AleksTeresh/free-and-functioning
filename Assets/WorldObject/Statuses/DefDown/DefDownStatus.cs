using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statuses
{
    public class DefDownStatus : Status
    {
        public float meleeDefenceDebuff = 10;
        public float rangeDefenceDebuff = 10;
        public float abilityDefenceDebuff = 10;

        private float meleeSubstractedAmount;
        private float rangeSubstractedAmount;
        private float abilitySubstractedAmount;

        protected override void OnStatusStart()
        {
            if (target)
            {
                // make sure melee defence does not get below zero
                meleeSubstractedAmount = target.meleeDefence - meleeDefenceDebuff >= 0
                    ? meleeDefenceDebuff
                    : target.meleeDefence;
                target.meleeDefence -= meleeSubstractedAmount;

                // make sure range defence does not get below zero
                rangeSubstractedAmount = target.rangeDefence - rangeDefenceDebuff >= 0
                    ? rangeDefenceDebuff
                    : target.rangeDefence;
                target.rangeDefence -= rangeSubstractedAmount;

                // make sure ability defence does not get below zero
                abilitySubstractedAmount = target.abilityDefence - abilityDefenceDebuff >= 0
                    ? abilityDefenceDebuff
                    : target.abilityDefence;
                target.abilityDefence -= abilitySubstractedAmount;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.meleeDefence += meleeSubstractedAmount;
                target.rangeDefence += rangeSubstractedAmount;
                target.abilityDefence += abilitySubstractedAmount;
            }
        }
    }
}

