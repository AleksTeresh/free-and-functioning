namespace Statuses
{
    public class AttackUpStatus : Status
    {
        public int damageBuff = 10;

        protected override void OnStatusStart()
        {
            if (target)
            {
                target.damage += damageBuff;
            }
        }

        protected override void OnStatusEnd()
        {
            if (target)
            {
                target.damage -= damageBuff;
            }
        }
    }
}
