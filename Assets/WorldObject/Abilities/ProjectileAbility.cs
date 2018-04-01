using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class ProjectileAbility : Ability
    {
        public override void FireAbility()
        {
            Vector3 spawnPoint = user.GetProjectileSpawnPoint();
            Quaternion rotation = user.transform.rotation;

            user.FireProjectile(target, "AbilityProjectile", spawnPoint, rotation, range, damage, statuses);
        }
    }
}
