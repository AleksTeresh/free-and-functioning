using UnityEngine;

namespace Abilities
{
    public class ProjectileAbility : Ability
    {
        public string projectileName;

        protected override void FireAbility()
        {
            Vector3 spawnPoint = user.GetProjectileSpawnPoint();

            targets.ForEach(target =>
            {
                if (!target) return;

                var rotation = Quaternion.LookRotation(target.transform.position - user.transform.position);
                user.FireProjectile(target, projectileName, spawnPoint, rotation, range, damage, statuses);
            });
        }
    }
}
