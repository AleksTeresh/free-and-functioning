namespace Statuses
{
    public class SilenceStatus : AiStateStatus
    {
        protected override void OnStatusStart()
        {
            if (target && target is Unit)
            {
                var targetUnit = (Unit)target;

                SetAbilitiesBlock(targetUnit, true);
            }
        }

        protected override void OnStatusEnd()
        {
            if (target && target is Unit)
            {
                var targetUnit = (Unit)target;

                SetAbilitiesBlock(targetUnit, false);
            }
        }

        private void SetAbilitiesBlock(Unit targetUnit, bool blocked)
        {
            foreach (var ability in targetUnit.GetAbilityAgent().abilities)
            {
                ability.blocked = blocked;
            }

            foreach (var ability in targetUnit.GetAbilityAgent().abilitiesMulti)
            {
                ability.blocked = blocked;
            }
        }
    }
}
