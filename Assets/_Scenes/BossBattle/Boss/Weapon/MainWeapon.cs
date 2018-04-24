using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class MainWeapon : BossPart {

    [Header("Weapon and Attack")]
    public int multiDamageMinValue = 1;
    public float meleeWeaponRange = 5;
    public int meleeDamage = 70;

    public override bool CanAttack()
    {
        return true;
    }

    public override bool CanAttackMulti()
    {
        return true;
    }

    public override bool IsMajor()
    {
        return true;
    }

    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);

        Vector3 currentPosition = transform.position;
        Vector3 currentEnemyPosition = WorkManager.GetTargetClosestPoint(this, target);
        Vector3 direction = currentEnemyPosition - currentPosition;

        if (direction.sqrMagnitude < this.meleeWeaponRange * this.meleeWeaponRange)
        {
            target.TakeDamage(meleeDamage, AttackType.Melee);
        }
        else
        {
            Vector3 spawnPoint = GetProjectileSpawnPoint();

            FireProjectile(target, "DamageDealerProjectile", spawnPoint, damage);
        }
    }

    protected override void UseWeaponMulti(List<WorldObject> targets)
    {
        base.UseWeaponMulti(targets);
        Vector3 spawnPoint = GetProjectileSpawnPoint();

        int dividedDamage = Mathf.Max(multiDamageMinValue, damageMulti / targets.Count);
        targets.ForEach(p =>
        {
            var rotation = Quaternion.LookRotation(p.transform.position - transform.position);
            FireProjectile(p, "DamageDealerLightProjectile", spawnPoint, rotation, dividedDamage);
        });
    }

    public override Vector3 GetProjectileSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;
        // spawnPoint.x += (2.1f * transform.forward.x);
        // spawnPoint.y += 1.4f;
        // spawnPoint.z += (2.1f * transform.forward.z);

        return spawnPoint;
    }
}
