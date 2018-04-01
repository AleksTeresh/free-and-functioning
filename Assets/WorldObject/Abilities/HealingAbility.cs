using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abilities
{
    class HealingAbility : Ability
    {
        public override bool IsHealingAbility()
        {
            return true;
        }

        public override void FireAbility()
        {
            //Player player = user.GetPlayer();

        }

        public override void FireAbilityMulti()
        {
            // choose all allies in range
        }
    }
}
