using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuffer : Unit {
    public override bool CanAttack()
    {
        return true;
    }

    public override bool CanAttackMulti()
    {
        return false;
    }

    public override bool IsMajor()
    {
        return true;
    }

    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);
        Vector3 spawnPoint = GetProjectileSpawnPoint();

        FireProjectile(target, "DebufferProjectile", spawnPoint);
    }

    public override Vector3 GetProjectileSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.5f;

        return spawnPoint;
    }
}
