using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Abilities;
using Persistence;

public class UnitStateController : StateController
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Unit unit;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public WorldObject allyAbilityTarget;
    [HideInInspector] public WorldObject enemyAbilityTarget;
    [HideInInspector] public Vector3 aoeAbilityTarget;
	[HideInInspector] public Ability abilityToUse;
    
    protected override void AwakeObj()
    {
        base.AwakeObj();

        unit = GetComponent<Unit>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        aoeAbilityTarget = new Vector3();
    }

    public void SetupAI(bool aiActivation, List<Transform> wayPoints)
    {
        SetupAI(aiActivation);

        wayPointList = wayPoints;

        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    public new UnitStateControllerData GetData()
    {
        var baseData = base.GetData();

        return new UnitStateControllerData(
            baseData,
            wayPointList,
            nextWayPoint,
            allyAbilityTarget ? allyAbilityTarget.ObjectId : -1,
            enemyAbilityTarget ? enemyAbilityTarget.ObjectId : -1,
            unit ? unit.ObjectId : -1,
            aoeAbilityTarget,
            abilityToUse ? abilityToUse.GetData() : null
        );
    }

    public void SetData (UnitStateControllerData data)
    {
        base.SetData(data);

        wayPointList = data.wayPointList;
        nextWayPoint = data.nextWayPoint;
        allyAbilityTarget = data.allyAbilityTargetId != -1
            ? Player.GetObjectById(data.allyAbilityTargetId)
            : null;
        enemyAbilityTarget = data.enemyAbilityTargetId != -1
            ? Player.GetObjectById(data.enemyAbilityTargetId)
            : null;
        aoeAbilityTarget = data.aoeAbilityTarget;
        abilityToUse = data.abilityToUse != null
            ? unit.GetAbilityAgent().FindAbilityByName(data.abilityToUse.type)
            : null;

        var unitObj = data.controlledUnitId != -1
            ? Player.GetObjectById(data.controlledUnitId)
            : null;
        if (unitObj)
        {
            unit = unitObj.GetComponent<Unit>();
        }
    }
}
