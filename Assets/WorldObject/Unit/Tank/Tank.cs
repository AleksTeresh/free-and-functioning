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
        Vector3 spawnPoint = transform.position;
        spawnPoint.x += (2.1f * transform.forward.x);
        spawnPoint.y += 1.4f;
        spawnPoint.z += (2.1f * transform.forward.z);
        GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetWorldObject("TankProjectile"), spawnPoint, transform.rotation);
        Projectile projectile = gameObject.GetComponentInChildren<Projectile>();
        projectile.Player = this.player;
        projectile.SetRange(0.9f * weaponRange);
        projectile.SetTarget(target);
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
}
