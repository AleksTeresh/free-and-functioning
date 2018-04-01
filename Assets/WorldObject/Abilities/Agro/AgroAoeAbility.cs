using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statuses;
using RTS;

namespace Abilities
{
    public class AgroAoeAbility : Ability
    {
        protected override void FireAbilityOnArea()
        {
            Vector3 spawnPoint = new Vector3(effectPosition.x, effectPosition.y + 2, effectPosition.z);

            CreateAreaOfEffect(spawnPoint, "AgroAreaOfEffect", 20, 0, statuses);
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

