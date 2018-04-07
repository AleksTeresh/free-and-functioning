namespace Abilities
{
    public class ShieldAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            targets.ForEach(target =>
            {
                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
