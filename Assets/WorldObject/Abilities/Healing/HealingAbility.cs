using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace Abilities
{
    class HealingAbility : Ability
    {
        protected override void FireAbility()
        {
            //InflictStatuses(target);

            targets.ForEach(target =>
            {
                target.TakeHeal(damage);

                ParticleSystem vfxPrefab = ResourceManager.GetAbilityVfx("HealingAbilityEffect");
                ParticleSystem vfxInstance = Instantiate(vfxPrefab, target.transform.position, Quaternion.identity);
                Destroy(vfxInstance.gameObject, vfxInstance.main.startLifetime.constantMax + vfxInstance.main.duration);
            });
        }
    }
}
