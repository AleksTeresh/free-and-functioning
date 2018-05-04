using System.Collections.Generic;
using UnityEngine;
using Events;

namespace RTS
{
	public static class HotkeyUnitSelector
	{
		private static string[] hotkeys = new string[] {
			"Select1",
			"Select2",
			"Select3",
			"Select4",
			"Select5"
		};

		public static void HandleInput (Player player, HUD hud, Camera camera)
		{
			if (player.selectedAllyTargettingAbility == null) {
                for (int i = 0; i < hotkeys.Length; i++) {
					if (Input.GetButtonDown (hotkeys [i]) || Gamepad.GetButtonDown(hotkeys[i])) {
						if (Input.GetButton ("SelectionModifier") || Gamepad.GetButton("SelectionModifier"))
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

				if (Input.GetButtonDown("SelectAll") || Gamepad.GetButtonDown("SelectAll") )
				{
					UnitSelectionManager.SelectAllUnits (player, hud);
                    EventManager.TriggerEvent("SelectAllUnits");
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
