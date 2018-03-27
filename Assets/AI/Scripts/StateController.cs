﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Unit unit;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public WorldObject chaseTarget;
    [HideInInspector] public float stateTimeElapsed;

    private bool aiActive;


    void Awake()
    {
        unit = GetComponent<Unit>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetupAI(bool aiActivation, List<Transform> wayPoints)
    {
        wayPointList = wayPoints;
        aiActive = aiActivation;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }
    /*
    void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }
    */
    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}
