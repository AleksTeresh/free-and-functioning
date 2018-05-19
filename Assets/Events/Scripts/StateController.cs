using System;
using System.Collections.Generic;
using UnityEngine;
using Dialog;
using Formation;
using Persistence;
using RTS;

namespace Events
{
    public class StateController : MonoBehaviour
    {
        public State currentState;

        [HideInInspector] public HUD hud;
        [HideInInspector] public Player player;
        [HideInInspector] public DialogManager dialogManager;
        [HideInInspector] public FormationManager formationManager;
        [HideInInspector] public TargetManager targetManager;

        [HideInInspector] public float timeInState = 0f;

        protected virtual void AwakeObj()
        {
            player = GetComponent<Player>();
            hud = player.GetComponentInChildren<HUD>();
            dialogManager = player.GetComponentInChildren<DialogManager>();
            formationManager = player.GetComponentInChildren<FormationManager>();
            targetManager = player.GetComponentInChildren<TargetManager>();
        }

        private void Start()
        {
            AwakeObj();

            if (currentState)
            {
                currentState.EnterState(this);
            }
        }

        protected virtual void Update()
        {
            if (currentState)
            {
                timeInState += Time.deltaTime;
                currentState.UpdateState(this);
            }
        }

        public void TransitionToState(State nextState)
        {
            if (currentState != nextState)
            {
                timeInState = 0;

                currentState.ExitState(this);
                nextState.EnterState(this);
                currentState = nextState;
            }
        }

        public EventStateControllerData GetData()
        {
            var data = new EventStateControllerData();

            data.timeInState = timeInState;
            data.currentState = currentState.name.Contains("(")
                ? currentState.name.Substring(0, currentState.name.IndexOf("(")).Trim()
                : currentState.name;

            return data;
        }

        public void SetData(EventStateControllerData data)
        {
            timeInState = data.timeInState;

            if (data.currentState != null && data.currentState != "")
            {
                currentState = GameObject.Instantiate(ResourceManager.GetEventState(data.currentState));
                currentState.name = currentState.name.Contains("(")
                ? currentState.name.Substring(0, currentState.name.IndexOf("(")).Trim()
                : currentState.name;
            }
        }
    }
}
