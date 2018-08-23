using RTS.Constants;
using UnityEngine;

namespace RTS
{
    public static class HotkeyAllyAbilityTargetSelector
    {
        private static string[] hotkeys = new string[] {
            InputNames.SELECT1,
            InputNames.SELECT2,
            InputNames.SELECT3,
            InputNames.SELECT4,
            InputNames.ALLY_ABILITY_TO_ALL
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
                        if (hotkey == InputNames.ALLY_ABILITY_TO_ALL)
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

                if (Input.GetButtonDown(InputNames.CANCEL))
                {
                    player.selectedAllyTargettingAbility = null;
                    player.selectedAlliesTargettingAbility = null;
                }
            }
        }

        private static void UseAbilityOnAlly(Player player, int hotkey)
        {
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
