using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Abilities;
using RTS;
using Persistence;

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

    public Ability FindAbilityByName (string name)
    {
        var foundAbility = abilities.First(ability => ability.name == name);

        if (foundAbility == null)
        {
            foundAbility = abilitiesMulti.First(ability => ability.name == name);
        }

        return foundAbility;
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

    public AbilityAgentData GetData ()
    {
        var data = new AbilityAgentData();

        data.abilities = new List<Ability>(abilities)
            .Select(ability => ability.GetData())
            .ToArray();
        data.abilitiesMulti = new List<Ability>(abilitiesMulti)
            .Select(ability => ability.GetData())
            .ToArray();

        return data;
    }
}
