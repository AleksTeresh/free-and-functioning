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
            "Select5",
            "Attack Mode"
        };

        public static void HandleInput(Player player, HUD hud)
        {
            if (player.selectedAllyTargettingAbility != null)
            {
                for (int i = 0; i < hotkeys.Length; i++)
                {
                    if (Input.GetButtonDown(hotkeys[i]))
                    {
                        string hotkey = hotkeys[i];

                        // If player hits space use on all allies
                        if (hotkey == "Attack Mode")
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


            if (allyTarget != null)
            {
                InputToCommandManager.AllyAbilityTargetSelectionToState(
                    player.SelectedObject.GetStateController(), player.selectedAllyTargettingAbility, allyTarget);

                ResetAbilitySelection(player);
            }
        }

        private static void UseAbilityOnAllies(Player player)
        {
            InputToCommandManager.AlliesAbilityTargetSelectionToState(
                player.SelectedObject.GetStateController(), player.selectedAlliesTargettingAbility);

            ResetAbilitySelection(player);
        }

        private static void ResetAbilitySelection(Player player)
        {
            player.selectedAllyTargettingAbility = null;
            player.selectedAlliesTargettingAbility = null;
        }
    }
}
