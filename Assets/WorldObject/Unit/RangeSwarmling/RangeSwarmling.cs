using UnityEngine;

public class RangeSwarmling : Unit {

    protected override void Start()
    {
        base.Start();
    }
		

    public override bool CanAttack()
    {
        return true;
    }

    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);
        Vector3 spawnPoint = GetSpawnPoint();

        FireProjectile(target, "SwarmlingProjectile", spawnPoint, damage);
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;
        spawnPoint.y += 1.4f;

        return spawnPoint;
    }
}
