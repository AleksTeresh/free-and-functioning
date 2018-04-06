using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statuses
{
    public class LongAndStrongStatus : Status
    {
        public int meleeDamageBuff = 10;
        public float rechargeTimeBuff = 10;

        protected override void OnStatusStart()
        {
            if (target)
            {
                target.damage += meleeDamageBuff;
                target.weaponRechargeTime += rechargeTimeBuff;

                target.ResetCurrentWeaponChargeTime();
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.damage -= meleeDamageBuff;
                target.weaponRechargeTime -= rechargeTimeBuff;

                target.ResetCurrentWeaponChargeTime();
            }
        }
    }

}

