﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;
using RTS;

namespace Abilities
{
    public class AreaOfEffect : MonoBehaviour
    {
        public SpriteRenderer Sprite { get; private set; }

        [HideInInspector] public int damage;
        [HideInInspector] public Status[] statuses;
        public float delay;
        public bool affectsFriends;
        public bool affectsSelf;
        [HideInInspector] public WorldObject creator;

        private float timeSinceCreated;

        void Awake()
        {
            Sprite = GetComponent<SpriteRenderer>();

            timeSinceCreated = 0;
        }

        void Update()
        {
            timeSinceCreated += Time.deltaTime;

            if (timeSinceCreated > delay)
            {
                var nearbyObjects = WorkManager.FindNearbyObjects(transform.position, transform.localScale.x / 2);
                HandleEffect(nearbyObjects);

                // destroy the area of effect
                Destroy(gameObject);
            }
        }

        public void SetRadius(float radius)
        {
            transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        }

        private void HandleEffect(List<WorldObject> affectedObjects)
        {
            InflictDamage(affectedObjects);
            InflictStatuses(affectedObjects);
        }

        private void InflictDamage(List<WorldObject> affectedObjects)
        {
            affectedObjects.ForEach(target =>
            {
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            });
        }

        private void InflictStatuses(List<WorldObject> affectedObjects)
        {
            affectedObjects.ForEach(target =>
            {
                if (target != null)
                {
                    // if the AoE does not affect friendly units and target is friendly, skip the target
                    if (creator && !affectsFriends && target.GetPlayer().username == creator.GetPlayer().username)
                    {
                        return;
                    }

                    // if the AoE does not affect its creator and target is the creator, skip the target
                    if (creator && !affectsSelf && target.ObjectId == creator.ObjectId)
                    {
                        return;
                    }

                    // otherwise inflict statuses upon the target
                    for (int i = 0; i < statuses.Length; i++)
                    {
                        StatusManager.InflictStatus(creator, statuses[i], target);
                    }
                }
            });
        }
    }
}