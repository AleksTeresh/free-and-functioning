namespace Abilities
{
    public class StatusAbility : AbilityWithVFX
    {
        protected override void FireAbility()
        {
            targets.ForEach(target =>
            {
                if (!target) return;

                InflictStatuses(target);
                PlayAbilityVFX(target);
            });
        }
    }
}
