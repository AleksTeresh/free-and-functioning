using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class ProjectileAbility : Ability
    {
        protected override void FireAbility()
        {
            Vector3 spawnPoint = user.GetProjectileSpawnPoint();

            targets.ForEach(target =>
            {
                var rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                user.FireProjectile(target, "AbilityProjectile", spawnPoint, rotation, range, damage, statuses);
            });
        }
    }
}
