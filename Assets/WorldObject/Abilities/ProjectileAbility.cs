using UnityEngine;

namespace Abilities
{
    public class ProjectileAbility : Ability
    {
        public string projectileName;

        protected override void FireAbility()
        {
            Vector3 spawnPoint = user.GetProjectileSpawnPoint();

            int dividedDamage = Mathf.Max(multiDamageMinValue, damage / targets.Count);
            targets.ForEach(target =>
            {
                if (!target) return;

                var rotation = Quaternion.LookRotation(target.transform.position - user.transform.position);
                user.FireProjectile(target, projectileName, spawnPoint, rotation, range, dividedDamage, statuses);
            });
        }
    }
}
