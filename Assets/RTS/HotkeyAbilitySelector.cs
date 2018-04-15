﻿using UnityEngine;

namespace RTS
{
    public static class HotkeyAbilitySelector
    {
        private static string[] hotkeys = new string[]
        {
            "Ability1",
            "Ability2",
            "Ability3",
            "Ability4",
            "Ability5"
        };

        public static void HandleInput(Player player, TargetManager targetManager)
        {
            if (player.SelectedObject != null && player.SelectedObject is Unit)
            {
                Unit selectedUnit = (Unit)player.SelectedObject;

                for (int i = 0; i < hotkeys.Length; i++)
                {
                    if (Input.GetButtonDown(hotkeys[i]) || Gamepad.GetButtonDown(hotkeys[i]))
                    {
						InputToCommandManager.AbilityHotkeyToState(targetManager, selectedUnit.GetStateController(), i);

                        return;
                    }
                }
            }
        }
    }
}
