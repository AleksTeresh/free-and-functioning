﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class EventManager
    {
        private volatile Dictionary<string, UnityEvent> eventDictionary;

        private static volatile EventManager eventManager;

        private EventManager() {}

        private static object syncRoot = new System.Object();
        public static EventManager instance
        {
            get
            {
                if (eventManager == null)
                {
                    lock (syncRoot)
                    {
                        eventManager = new EventManager();

                        if (eventManager == null)
                        {
                            Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                        }
                        else
                        {
                            Debug.Log("EventManager is to be initialized");
                            eventManager.Init();
                        }
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) return;
            UnityEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}