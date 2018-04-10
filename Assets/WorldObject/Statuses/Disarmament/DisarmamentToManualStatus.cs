using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statuses
{
    public class DisarmamentToManualStatus : Status
    {
        private int substractedAmount;

        protected override void OnStatusStart()
        {
            if (target)
            {
                substractedAmount = target.damage;
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

