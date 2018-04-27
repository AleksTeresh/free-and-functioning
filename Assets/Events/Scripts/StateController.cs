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
            currentState.EnterState(this);
        }

        protected virtual void Update()
        {
            currentState.UpdateState(this);
        }

        public void TransitionToState(State nextState)
        {
            if (currentState != nextState)
            {
                currentState.ExitState(this);
                nextState.EnterState(this);
                currentState = nextState;
            }
        }
    }
}
