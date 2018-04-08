using UnityEngine;

namespace Statuses
{
    public class DotStatus : Status
    {
        public int totalDamage = 100;

        // Public only for debug purposes 
        private float tickCooldownCounter = 0.0f;
        private readonly float tickCooldownDuration = 1.0f;
        private int tickDamage;
        private bool isTickReady = true;

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
