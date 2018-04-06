using System.Collections.Generic;
using UnityEngine;

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

		public static void HandleInput (Player player, HUD hud)
		{
			if (player.selectedAllyTargettingAbility == null) {
				for (int i = 0; i < hotkeys.Length; i++) {
					if (Input.GetButtonDown (hotkeys [i])) {
						SelectPlayerUnitByHotkeyIndex (player, hud, i);

						return;
					}
				}
			}
		}

		private static void SelectPlayerUnitByHotkeyIndex (Player player, HUD hud, int hotkey)
		{
			var units = player.GetUnits ();
			WorldObject unitToSelect = player.unitMapping.FindUnitByHotkey (units, hotkey);

			if (unitToSelect) {
				ResetPlayerUnitsSelection (units, hud);

				player.SelectedObject = unitToSelect;
				unitToSelect.SetSelection (true, hud.GetPlayingArea ());
			}

		}

		private static void ResetPlayerUnitsSelection (List<Unit> units, HUD hud)
		{
			units.ForEach (p => p.SetSelection (false, hud.GetPlayingArea ()));
		}
	}
}
