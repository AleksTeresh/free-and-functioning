using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Abilities;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    [HideInInspector] private State defaultState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Unit unit;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
	[HideInInspector] public WorldObject chaseTarget;
    [HideInInspector] public WorldObject allyAbilityTarget;
    [HideInInspector] public WorldObject enemyAbilityTarget;
    [HideInInspector] public Vector3 aoeAbilityTarget;
    [HideInInspector] public TargetManager targetManager;
    [HideInInspector] public List<WorldObject> nearbyEnemies;
    [HideInInspector] public List<WorldObject> nearbyAllies; // including self
    // [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public bool attacking;
	[HideInInspector] public Ability abilityToUse;

    private bool aiActive;
    
    void Awake()
    {
        unit = GetComponent<Unit>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetManager = transform.root.GetComponentInChildren<TargetManager>();

        aoeAbilityTarget = new Vector3();

        // for now the default state is the one that a unit has on Awake(),
        // later defaultState can be made a separate public variable to be set in the Inspector
        defaultState = currentState;
    }

    private void Start()
    {
        nearbyEnemies = new List<WorldObject>();
        nearbyAllies = new List<WorldObject>();
        attacking = false;
    }

    public void SetupAI(bool aiActivation)
    {
        SetupAI(aiActivation, new List<Transform>());
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

    public State GetDefaultState()
    {
        return defaultState;
    }

    protected virtual void Update()
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
            // OnExitState();
        }
    }
    /*
    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
        */
}
