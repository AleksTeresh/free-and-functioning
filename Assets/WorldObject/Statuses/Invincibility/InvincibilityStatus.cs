namespace Statuses
{
    public class InvincibilityStatus : Status
    {
        protected override void OnStatusStart()
        {
            if (target)
            {
                target.isInvincible = true;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.isInvincible = false;
            }
        }
    }
}
