﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Dialog;
using Formation;
using Persistence;
using RTS;
using System.Linq;

namespace Events
{
    public class StateController : MonoBehaviour
    {
        public State currentState;

        public EventObject[] eventObjects;

        [HideInInspector] public HUD hud;
        [HideInInspector] public Player player;
        [HideInInspector] public DialogManager dialogManager;
        [HideInInspector] public FormationManager formationManager;
        [HideInInspector] public TargetManager targetManager;

        [HideInInspector] public float timeInState = 0f;

        [HideInInspector] public AudioClip currentAudioClip;

        private bool sceneWasLoaded = false;

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

            if (currentState && !sceneWasLoaded)
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
            data.dialogManagerData = dialogManager.GetData();
            data.eventObjectIds = (eventObjects != null ? new List<EventObject>(eventObjects) : new List<EventObject>())
                .Select(eventObject => eventObject.ObjectId).ToList();

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

                var bgmAction = currentState.enterActions.Last<Action>(action => action is PlayBgmAction);
                if (bgmAction)
                {
                    bgmAction.Act(this);
                }
            }

            List<EventObject> loadedEventObjects = new List<EventObject>();

            foreach(int eventObjectId in data.eventObjectIds)
            {
                EventObject[] eventObjects = FindObjectsOfType<EventObject>();

                foreach(EventObject eventObject in eventObjects)
                {
                    if (eventObject.ObjectId == eventObjectId)
                    {
                        loadedEventObjects.Add(eventObject);
                    }
                }
            };

            eventObjects = loadedEventObjects.ToArray();

            AwakeObj();

            dialogManager.SetData(data.dialogManagerData);

            sceneWasLoaded = true;
        } 

        public EventObject GetEventObjectByName(string eventObjectName)
        {
            foreach (EventObject eventObject in eventObjects)
            {
                if (eventObject.name == eventObjectName)
                {
                    return eventObject;
                }
            }

            throw new EventObjectNotRegisteredException(String.Format("Event object with name \"{0}\" is not registered", eventObjectName));
        }

        public bool IsEventObjectActivated(string eventObjectName)
        {
            try
            {
                EventObject eventObject = GetEventObjectByName(eventObjectName);

                return eventObject.IsActivated();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);

                return false;
            }
        }

        public bool IsEventObjectTriggered(string eventObjectName)
        {
            try
            {
                return GetEventObjectByName(eventObjectName).triggerred;
            } catch (Exception e)
            {
                Debug.LogWarning(e);
            
                return false;
            }
        }

        public bool IsEventObjectCompleted(string eventObjectName)
        {
            try
            {
                return GetEventObjectByName(eventObjectName).completed;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);

                return false;
            }
        }

        internal class EventObjectNotRegisteredException: Exception
        {
            public EventObjectNotRegisteredException(string message): base(message){}
        }
    }
}
