using Persistence;
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
            spawnTimer,
            building ? building.ObjectId : -1
        );
    }

    public void SetData(BuildingStateControllerData data)
    {
        base.SetData(data);

        spawnTimer = data.spawnTimer;
        var buildingObj = data.controlledBuildingId != -1
            ? Player.GetObjectById(data.controlledBuildingId)
            : null;

        if (buildingObj)
        {
            building = buildingObj.GetComponent<Building>();
        }
    }
}
