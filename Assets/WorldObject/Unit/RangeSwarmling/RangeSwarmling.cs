using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class RangeSwarmling : Unit {

    public override void SaveDetails(JsonWriter writer)
    {
        base.SaveDetails(writer);
        SaveManager.WriteQuaternion(writer, "AimRotation", aimRotation);
    }

    protected override void Start()
    {
        base.Start();

        if (stateController)
        {
            List<Transform> wayPoints = new List<Transform>();


            var pp1 = Instantiate(ResourceManager.GetWorldObject("PatrolPoint"));
            pp1.transform.position = Vector3.MoveTowards(transform.position, new Vector3(-100, 0, -100), 20);
            var pp2 = Instantiate(ResourceManager.GetWorldObject("PatrolPoint"));
            pp2.transform.position = Vector3.MoveTowards(transform.position, new Vector3(100, 0, 100), 20);

            wayPoints.Add(pp1.transform);
            wayPoints.Add(pp2.transform);

            stateController.SetupAI(true, wayPoints);
        }
    }
		

    public override bool CanAttack()
    {
        return true;
    }

    protected override void UseWeapon(WorldObject target)
    {
        base.UseWeapon(target);
        Vector3 spawnPoint = GetSpawnPoint();

        FireProjectile(target, "SwarmlingProjectile", spawnPoint);
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
        spawnPoint.x += (2.1f * transform.forward.x);
        spawnPoint.y += 1.4f;
        spawnPoint.z += (2.1f * transform.forward.z);

        return spawnPoint;
    }
}
