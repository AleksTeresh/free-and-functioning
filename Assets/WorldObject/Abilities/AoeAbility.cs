using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;
using RTS;

namespace Abilities
{
    public class AoeAbility : Ability
    {
        public Vector3 effectPosition;
        public AreaOfEffect aoe;
        public float radius;

        public int aoeDamage;
        public Status[] aoeStatuses;

        public void UseOnArea(Vector3 position)
        {
            if (isReady)
            {
                this.effectPosition = position;

                HandleAbilityUse();

                FireAbilityOnArea();
            }
        }

        protected void FireAbilityOnArea()
        {
            Vector3 spawnPoint = new Vector3(effectPosition.x, effectPosition.y + 2, effectPosition.z);

            CreateAreaOfEffect(spawnPoint, aoe.name, radius, aoeDamage, aoeStatuses);
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
            aoe.SetRadius(radius);
        }
    }
}

