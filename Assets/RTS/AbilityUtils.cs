using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;

namespace RTS
{
    public static class AbilityUtils
    {
        public static bool CanUseAbilitySlot(Ability[] abilities, Ability[] multiAbilities, int slotIdx)
        {
            return !IsAbilitySlotEmpty(abilities, multiAbilities, slotIdx) &&
                   !IsAbilitySlotInCooldown(abilities, multiAbilities, slotIdx);
        }

        public static bool IsAbilitySlotEmpty(Ability[] abilities, Ability[] multiAbilities, int slotIdx)
        {
            Ability ability = FindAbilityByIndex(abilities, slotIdx);
            Ability multiAbility = FindAbilityByIndex(multiAbilities, slotIdx);

            return !ability && !multiAbility;
        }

        public static bool IsAbilitySlotInCooldown(Ability[] abilities, Ability[] multiAbilities, int slotIdx)
        {
            Ability abilityInCooldown = FindAbilityInCooldown(abilities, multiAbilities, slotIdx);

            return !!abilityInCooldown;
        }

        public static float GetAbilitySlotCooldownRation(Ability[] abilities, Ability[] multiAbilities, int slotIdx)
        {
            Ability abilityInCooldown = FindAbilityInCooldown(abilities, multiAbilities, slotIdx);

            if (abilityInCooldown)
            {
                return abilityInCooldown.cooldownTimer / abilityInCooldown.cooldown;
            }

            return 1.0f;
        }

        public static Ability FindAbilityInCooldown(Ability[] abilities, Ability[] multiAbilities, int slotIdx)
        {
            Ability ability = FindAbilityByIndex(abilities, slotIdx);
            Ability multiAbility = FindAbilityByIndex(multiAbilities, slotIdx);

            if (ability && !ability.IsReady())
            {
                return ability;
            }
            else if (multiAbility && !multiAbility.IsReady())
            {
                return multiAbility;
            }

            return null;
        }

        public static Ability FindAbilityByIndex(Ability[] abilities, int slotIdx)
        {
            if (slotIdx < abilities.Length)
            {
                return abilities[slotIdx];
            }

            return null;
        }

        public static Ability FindAbilityByName(string name, Ability[] abilities)
        {
            foreach (var ability in abilities)
            {
                if (ability.name == name)
                {
                    return ability;
                }
            }

            return null;
        }

        public static Ability ChooseRandomReadyAbility(Ability[] abilities)
        {
            List<Ability> readyAbilities = new List<Ability>();

            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i].IsReady())
                {
                    readyAbilities.Add(abilities[i]);
                }
            }

            if (readyAbilities.Count > 0)
            {
                return readyAbilities[Random.Range(0, readyAbilities.Count)];
            }

            return null;
        }

        public static void PlayAbilityVfx(string vfxName, Vector3 target, Vector3 scale, Quaternion rotation)
        {
//            if (target != null) return;

            ParticleSystem vfxPrefab = ResourceManager.GetAbilityVfx(vfxName);
            ParticleSystem vfxInstance = MonoBehaviour.Instantiate(vfxPrefab, target, rotation);
            vfxInstance.transform.localScale = scale;
            MonoBehaviour.Destroy(vfxInstance.gameObject,
                vfxInstance.main.startLifetime.constantMax + vfxInstance.main.duration);
        }
    }
}