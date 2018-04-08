using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statuses
{ 
    public class AttackDownStatus : Status
    {
        public int damageDebuff = 10;

        private int substractedAmount;

        protected override void OnStatusStart()
        {
            if (target)
            {
                // make sure damage does not get below zero
                substractedAmount = target.damage - damageDebuff >= 0
                    ? damageDebuff
                    : target.damage;
                target.damage -= substractedAmount;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.damage += substractedAmount;
            }
        }
    }
}

