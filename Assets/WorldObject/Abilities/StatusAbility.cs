namespace Abilities
{
    public class StatusAbility : AbilityWithVFX
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
