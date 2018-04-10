using Abilities;

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
            Ability multiAbility = FindAbilityByIndex(abilities, slotIdx);

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
    }
}
