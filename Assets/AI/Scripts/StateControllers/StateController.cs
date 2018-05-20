using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Persistence;
using RTS;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    [HideInInspector] protected State defaultState;

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

    public void MarkChaseTarget()
    {
        if (chaseTarget && controlledObject.GetPlayer() && controlledObject.GetPlayer().human)
        {
            chaseTarget.MarkAsChasedTarget();
        }
    }

    private void DoExpensiveStateUpdate()
    {
        if (aiActive && currentState)
        {
            currentState.UpdateStateExpensive(this);
        }
    }

    public StateControllerData GetData ()
    {
        string currentStateName = currentState.name;
        string defaultStateName = defaultState.name;

        var data = new StateControllerData(
            currentStateName.Contains("(") ? currentStateName.Substring(0, currentStateName.IndexOf("(")).Trim() : currentStateName,
            defaultStateName.Contains("(") ? defaultStateName.Substring(0, defaultStateName.IndexOf("(")).Trim() : defaultStateName,
            chaseTarget ? chaseTarget.ObjectId : -1,
            controlledObject ? controlledObject.ObjectId : -1,
            attacking,
            aiActive
        );

        return data;
    }

    public void SetData (StateControllerData data)
    {
        if (data == null) return;

        currentState = GameObject.Instantiate(ResourceManager.GetAiState(data.currentState));
        defaultState = GameObject.Instantiate(ResourceManager.GetAiState(data.defaultState));

        string currentStateName = currentState.name;
        string defaultStateName = defaultState.name;

        currentState.name = currentStateName.Contains("(") ? currentStateName.Substring(0, currentStateName.IndexOf("(")).Trim() : currentStateName;
        defaultState.name = defaultStateName.Contains("(") ? defaultStateName.Substring(0, defaultStateName.IndexOf("(")).Trim() : defaultStateName;

        chaseTarget = data.chaseTargetId != -1
            ? Player.GetObjectById(data.chaseTargetId)
            : null;
        controlledObject = data.controlledObjectId != -1
           ? Player.GetObjectById(data.controlledObjectId)
           : null;
        attacking = data.attacking;
        aiActive = data.aiActive;
    }
}

