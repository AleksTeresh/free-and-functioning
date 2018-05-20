using System.Collections.Generic;
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
            "AllyAbilityToAll"
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
                        if (hotkey == "AllyAbilityToAll")
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

                if (Input.GetButtonDown("Cancel"))
                {
                    player.selectedAllyTargettingAbility = null;
                    player.selectedAlliesTargettingAbility = null;
                }
            }
        }

        private static void UseAbilityOnAlly(Player player, int hotkey)
        {
//            Unit allyTarget = player.FindUnitByIdx(unitIndex);
			WorldObject allyTarget = player.unitMapping.FindUnitByHotkey (player.GetUnits(), hotkey);

            AbilityUtils.ApplyAllyAbilityToTarget(allyTarget, player);
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

                AbilityUtils.ResetAbilitySelection(player);
            }
        }
    }
}
