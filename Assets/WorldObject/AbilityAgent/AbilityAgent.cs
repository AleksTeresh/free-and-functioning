using UnityEngine;
using System.Collections;
using Abilities;
using RTS;

public class AbilityAgent : MonoBehaviour
{
    [HideInInspector] public Ability[] abilities;
    [HideInInspector] public Ability[] abilitiesMulti;

    public Ability FindAbilityByIndex(int abilityIndex)
    {
        if (abilityIndex < abilities.Length)
        {
            return abilities[abilityIndex];
        }

        return null;
    }

    public Ability FindAbilityMultiByIndex(int abilityIndex)
    {
        if (abilityIndex < abilitiesMulti.Length)
        {
            return abilitiesMulti[abilityIndex];
        }

        return null;
    }

    public bool CanUseAbilitySlot(int slotIdx)
    {
        return AbilityUtils.CanUseAbilitySlot(abilities, abilitiesMulti, slotIdx);
    }

    public Ability GetFirstReadyAbility()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (CanUseAbilitySlot(i))
            {
                return abilities[i];
            }
        }

        return null;
    }

    public Ability GetFirstReadyMultiAbility()
    {
        for (int i = 0; i < abilitiesMulti.Length; i++)
        {
            if (CanUseAbilitySlot(i))
            {
                return abilitiesMulti[i];
            }
        }

        return null;
    }

    public bool IsAnyAbilityPending()
    {
        foreach (var ability in abilities)
        {
            if (ability.isPending)
            {
                return true;
            }
        }

        foreach (var ability in abilitiesMulti)
        {
            if (ability.isPending)
            {
                return true;
            }
        }

        return false;
    }

    public void Init (Abilities.Abilities abilitiesWrapper, AbilitiesMulti abilitiesMultiWrapper)
    {
        if (abilitiesWrapper)
        {
            abilities = abilitiesWrapper.GetComponentsInChildren<Ability>();
        }

        if (abilitiesMultiWrapper)
        {
            abilitiesMulti = abilitiesMultiWrapper.GetComponentsInChildren<Ability>();
        }
    }
}
