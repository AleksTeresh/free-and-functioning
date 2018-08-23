using UnityEngine;
using Events;
using RTS.Constants;

namespace RTS
{
	public static class HotkeyUnitSelector
	{
		private static string[] hotkeys = new string[] {
			InputNames.SELECT1,
            InputNames.SELECT2,
            InputNames.SELECT3,
            InputNames.SELECT4,
        };

		public static void HandleInput (Player player, HUD hud, Camera camera)
		{
			if (player.selectedAllyTargettingAbility == null) {
                for (int i = 0; i < hotkeys.Length; i++) {
					if (Input.GetButtonDown (hotkeys [i]) || Gamepad.GetButtonDown(hotkeys[i])) {
						if (Input.GetButton (InputNames.SELECTION_MODIFIER) || Gamepad.GetButton(InputNames.SELECTION_MODIFIER))
						{
							HandleUnitHotkeyWithModifierPress (player, hud, i);
						}
						else 
						{
							HandleUnitHotkeyPress (camera, player, hud, i);
						}

						return;
					}
				}

				if (Input.GetButtonDown(InputNames.SELECT_ALL) || Gamepad.GetButtonDown(InputNames.SELECT_ALL) )
				{
					UnitSelectionManager.SelectAllUnits (player, hud);
                    EventManager.TriggerEvent(InputNames.SELECT_ALL_UNITS);
				}
			}
		}

        private static void HandleUnitHotkeyPress (Camera camera, Player player, HUD hud, int hotkey)
        {
            var units = player.GetUnits ();
			Unit unitToSelect = player.unitMapping.FindUnitByHotkey (units, hotkey);

            UnitSelectionManager.HandleUnitSelection(unitToSelect, player, camera, hud);
		}

		private static void HandleUnitHotkeyWithModifierPress (Player player, HUD hud, int hotkey)
		{
			Unit unitToAdd = player.unitMapping.FindUnitByHotkey (player.GetUnits(), hotkey);

            UnitSelectionManager.HandleUnitSelectionWithModifierPress(unitToAdd, player, hud);
		}
	}
}
