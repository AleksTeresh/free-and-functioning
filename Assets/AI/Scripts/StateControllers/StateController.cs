using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    [HideInInspector] private State defaultState;

    [HideInInspector] public WorldObject chaseTarget;

    [HideInInspector] public WorldObject controlledObject;
    [HideInInspector] public TargetManager targetManager;
    [HideInInspector] public List<WorldObject> nearbyEnemies;
    [HideInInspector] public List<WorldObject> nearbyAllies; // including self
    // [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public bool attacking;

    protected bool aiActive;

    protected virtual void AwakeObj()
    {
        // to be overriden
    }

    private void Start()
    {
        AwakeObj();

        controlledObject = GetComponent<WorldObject>();
        targetManager = transform.root.GetComponentInChildren<TargetManager>();

        // for now the default state is the one that a indicatedObject/building has on Awake(),
        // later defaultState can be made a separate public variable to be set in the Inspector
        defaultState = currentState;

        // nearbyEnemies = new List<WorldObject>();
        // nearbyAllies = new List<WorldObject>();
        attacking = false;

        InvokeRepeating("DoExpensiveStateUpdate", 0.5f, 0.2f);
    }

    public void SetupAI(bool aiActivation)
    {
        aiActive = aiActivation;
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

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            // OnExitState();
        }
    }

    private void DoExpensiveStateUpdate()
    {
        if (aiActive && currentState)
        {
            currentState.UpdateStateExpensive(this);
        }
    }
}
