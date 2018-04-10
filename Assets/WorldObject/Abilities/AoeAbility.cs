﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;
using RTS;

namespace Abilities
{
    public class AoeAbility : AbilityWithVFX
    {
        public AreaOfEffect aoe;
        public float radius;
        public float aoeDelay;
        public bool affectsFriends;
        public bool affectsSelf;

        private Vector3 targetPosition;

        public void Use(Vector3 position)
        {
            if (IsReady())
            {
                targetPosition = position;

                HandleAbilityUseStart();
            }
        }

        protected override void FireAbility()
        {
            Vector3 spawnPoint = new Vector3(targetPosition.x, targetPosition.y + 2, targetPosition.z);

            CreateAreaOfEffect(spawnPoint, aoe.name, radius, damage, statuses);
        }

        private void CreateAreaOfEffect(Vector3 position, string aoeName, float radius, int damage, Status[] statuses)
        {
            Quaternion aoeRotation = new Quaternion(90, 0, 0, 90);
            GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetWorldObject(aoeName), position, aoeRotation);
            AreaOfEffect aoe = gameObject.GetComponentInChildren<AreaOfEffect>();
            aoe.statuses = statuses;
            aoe.damage = damage;
            aoe.affectsFriends = false;
            aoe.affectsSelf = false;
            aoe.creator = user;
            aoe.attackType = attackType;
            aoe.delay = aoeDelay;
            aoe.affectsFriends = affectsFriends;
            aoe.affectsSelf = affectsSelf;
            aoe.vfxName = vfxName;
            aoe.SetRadius(radius);
        }
    }
}
