using System;
using System.Collections.Generic;
using UnityEngine;
using Dialog;
using Formation;

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

        private void Awake()
        {
            player = GetComponent<Player>();
            hud = player.GetComponentInChildren<HUD>();
            dialogManager = player.GetComponentInChildren<DialogManager>();
            formationManager = player.GetComponentInChildren<FormationManager>();
            targetManager = player.GetComponentInChildren<TargetManager>();
        }

        private void Start()
        {
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
    }
}
