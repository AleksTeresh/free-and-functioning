using UnityEngine;

namespace Statuses
{
    public class DotStatus : Status
    {
         public int totalDamage = 100;

        // Public only for debug purposes 
        public float tickCooldownCounter = 0.0f;
        public float tickCooldownDuration;
        public int tickDamage;
        public bool isTickReady = true;

        protected override void OnStatusStart()
        {
            tickCooldownCounter = 0.0f;
            isTickReady = true;
            tickDamage = (int)(totalDamage / maxDuration);
        }

        protected override void AffectTarget()
        {
            if (isTickReady)
            {
                Tick();
            }
            else
            {
                TickCooldown();
            }
        }

        private void Tick()
        {
            if (target)
            {
                target.TakeDamage(tickDamage, RTS.AttackType.Ultimate);

                isTickReady = false;
            }
        }

        private void TickCooldown()
        {
            tickCooldownCounter += Time.deltaTime;

            if (tickCooldownCounter >= tickCooldownDuration)
            {
                tickCooldownCounter = 0.0f;
                isTickReady = true;
            }
        }
    }
}
