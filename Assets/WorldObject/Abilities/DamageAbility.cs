using UnityEngine;

namespace Abilities
{
    class DamageAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            int dividedDamage = Mathf.Max(multiDamageMinValue, damage / targets.Count);
            Debug.Log("Damage ability was used");
            targets.ForEach(target =>
            {
                target.TakeDamage(dividedDamage, RTS.AttackType.Ability);
                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
