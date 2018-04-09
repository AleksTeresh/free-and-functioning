using UnityEngine;

namespace Abilities
{
    class DamageAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            Debug.Log("Damage ability was used");
            targets.ForEach(target =>
            {
                target.TakeDamage(damage, RTS.AttackType.Ability);
                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
