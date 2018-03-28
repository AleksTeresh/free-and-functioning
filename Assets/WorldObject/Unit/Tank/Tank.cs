using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class Tank : Unit
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
        return true;
    }

    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);
        Vector3 spawnPoint = GetSpawnPoint();

        FireProjectile(target, "TankProjectile", spawnPoint);
    }

    protected override void UseWeaponMulti(List<WorldObject> targets)
    {
        base.UseWeaponMulti(targets);
        Vector3 spawnPoint = GetSpawnPoint();

        targets.ForEach(p =>
        {
            var rotation = Quaternion.LookRotation(p.transform.position - transform.position);
            FireProjectile(p, "TankLightProjectile", spawnPoint, rotation);
        });
    }

    protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
    {
        base.HandleLoadedProperty(reader, propertyName, readValue);
        switch (propertyName)
        {
            case "AimRotation": aimRotation = LoadManager.LoadQuaternion(reader); break;
            default: break;
        }
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = transform.position;
        // spawnPoint.x += (2.1f * transform.forward.x);
        spawnPoint.y += 1.4f;
        // spawnPoint.z += (2.1f * transform.forward.z);

        return spawnPoint;
    }
}
