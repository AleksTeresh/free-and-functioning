using UnityEngine;
using RTS.Constants;

namespace RTS
{
    public static class HotkeyAbilitySelector
    {
        private static string[] hotkeys = new string[]
        {
            InputNames.ABILITY1,
            InputNames.ABILITY2,
            InputNames.ABILITY3,
            InputNames.ABILITY4,
            InputNames.ABILITY5
        };

        public static void HandleInput(Player player, TargetManager targetManager)
        {
            if (player.SelectedObject != null && player.SelectedObject is Unit)
            {
                Unit selectedUnit = (Unit)player.SelectedObject;

                for (int i = 0; i < hotkeys.Length; i++)
                {
                    if (Input.GetButtonDown(hotkeys[i]) ||  (Gamepad.GetButtonDown(hotkeys[i]) && !Gamepad.GetButton(InputNames.SELECTION_MODIFIER)))
                    {
						InputToCommandManager.AbilityHotkeyToState(targetManager, selectedUnit.GetStateController(), i);

                        return;
                    }
                }
            }
        }
    }
}
