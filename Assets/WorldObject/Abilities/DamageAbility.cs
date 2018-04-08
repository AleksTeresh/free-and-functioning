namespace Abilities
{
    class DamageAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            targets.ForEach(target =>
            {
                target.TakeDamage(damage, RTS.AttackType.Ability);
                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
