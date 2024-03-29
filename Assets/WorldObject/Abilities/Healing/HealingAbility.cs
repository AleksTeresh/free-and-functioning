﻿namespace Abilities
{
    class HealingAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            targets.ForEach(target =>
            {
                target.TakeHeal(damage);
                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
