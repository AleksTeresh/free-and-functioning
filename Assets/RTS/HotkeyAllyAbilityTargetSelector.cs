﻿using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public static class HotkeyAllyAbilityTargetSelector
    {
        private static string[] hotkeys = new string[] {
            "Select1",
            "Select2",
            "Select3",
            "Select4",
            "SelectAll"
        };

        public static void HandleInput(Player player, HUD hud)
        {
            if (player.selectedAllyTargettingAbility != null)
            {
                for (int i = 0; i < hotkeys.Length; i++)
                {
                    if (Input.GetButtonDown(hotkeys[i]) || Gamepad.GetButtonDown(hotkeys[i]))
                    {
                        string hotkey = hotkeys[i];

                        // If player hits space use on all allies
                        if (hotkey == "SelectAll")
                        {
                            UseAbilityOnAllies(player);
                        }
                        else
                        {
                            UseAbilityOnAlly(player, i);
                        }

                        return;
                    }
                }
            }
        }

        private static void UseAbilityOnAlly(Player player, int hotkey)
        {
//            Unit allyTarget = player.FindUnitByIdx(unitIndex);
			WorldObject allyTarget = player.unitMapping.FindUnitByHotkey (player.GetUnits(), hotkey);

            // make sure that a current selection (ability user) belongs to the player and is a Unit
            if (
                allyTarget != null &&
                player.SelectedObject && 
                player.SelectedObject.GetPlayer().username == player.username &&
                player.SelectedObject is Unit
            )
            {
                var abilityUser = (Unit)player.SelectedObject;

                InputToCommandManager.AllyAbilityTargetSelectionToState(
                    abilityUser.GetStateController(), player.selectedAllyTargettingAbility, allyTarget);

                ResetAbilitySelection(player);
            }
        }

        private static void UseAbilityOnAllies(Player player)
        {
            // make sure that a current selection (ability user) belongs to the player and is a Unit
            if (
                player.SelectedObject &&
                player.SelectedObject.GetPlayer().username == player.username &&
                player.SelectedObject is Unit
            )
            {
                var abilityUser = (Unit) player.SelectedObject;

                InputToCommandManager.AlliesAbilityTargetSelectionToState(
                abilityUser.GetStateController(), player.selectedAlliesTargettingAbility);

                ResetAbilitySelection(player);
            }
        }

        private static void ResetAbilitySelection(Player player)
        {
            player.selectedAllyTargettingAbility = null;
            player.selectedAlliesTargettingAbility = null;
        }
    }
}
