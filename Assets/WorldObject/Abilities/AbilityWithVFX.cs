using RTS;
using UnityEngine;

namespace Abilities
{
    public class AbilityWithVFX : Ability
    {
        public string vfxName;

        protected void PlayAbilityVFX(WorldObject target)
        {
            if (!target) return;

            ParticleSystem vfxPrefab = ResourceManager.GetAbilityVfx(vfxName);
            ParticleSystem vfxInstance = Instantiate(vfxPrefab, target.transform.position, Quaternion.identity);
            Destroy(vfxInstance.gameObject, vfxInstance.main.startLifetime.constantMax + vfxInstance.main.duration);
        }
    }
}
