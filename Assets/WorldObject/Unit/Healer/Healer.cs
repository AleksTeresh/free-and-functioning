using UnityEngine;

public class Healer : Unit
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

    }

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

        FireProjectile(target, "HealerProjectile", spawnPoint, damage);
    }
    /*
    protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        base.HandleLoadedProperty(reader, propertyName, readValue);
        switch (propertyName)
        {
            case "AimRotation": aimRotation = LoadManager.LoadQuaternion(reader); break;
            default: break;
        }
    } */

    public override Vector3 GetProjectileSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;
        // spawnPoint.x += (2.1f * transform.forward.x);
        spawnPoint.y += 1.4f;
        // spawnPoint.z += (2.1f * transform.forward.z);

        return spawnPoint;
    }
}
