using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;

namespace Statuses
{
    public class ShieldStatus : Status
    {
        public float meleeDefenceBuff = 10;
        public float rangeDefenceBuff = 10;

        protected override void OnStatusStart()
        {
            if (target)
            {
                target.meleeDefence += meleeDefenceBuff;
                target.rangeDefence += rangeDefenceBuff;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.meleeDefence -= meleeDefenceBuff;
                target.rangeDefence -= rangeDefenceBuff;
            }
        }
    }
}

