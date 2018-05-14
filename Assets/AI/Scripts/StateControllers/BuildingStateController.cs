using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildingStateController : StateController
{
    [HideInInspector] public Building building;

    [HideInInspector] public float spawnTimer;

    protected override void AwakeObj()
    {
        base.AwakeObj();

        building = GetComponent<Building>();
    }


    protected override void Update()
    {
        base.Update();

        spawnTimer += Time.deltaTime;
    }

    public new BuildingStateControllerData GetData()
    {
        var baseData = base.GetData();

        return new BuildingStateControllerData(
            baseData,
            spawnTimer
        );
    }

    public void SetData(BuildingStateControllerData data)
    {
        base.SetData(data);

        spawnTimer = data.spawnTimer;
    }
}
