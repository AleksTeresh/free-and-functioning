using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Abilities;

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
    
    protected override void Awake()
    {
        base.Awake();

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
}
