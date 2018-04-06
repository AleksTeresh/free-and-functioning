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

		public static void HandleInput (Player player, HUD hud, Camera camera)
		{
			if (player.selectedAllyTargettingAbility == null) {
				for (int i = 0; i < hotkeys.Length; i++) {
					if (Input.GetButtonDown (hotkeys [i])) {
						SelectPlayerUnitByHotkeyIndex (camera, player, hud, i);

						return;
					}
				}
			}
		}

		private static void SelectPlayerUnitByHotkeyIndex (Camera camera, Player player, HUD hud, int hotkey)
        {
            var units = player.GetUnits ();
			Unit unitToSelect = player.unitMapping.FindUnitByHotkey (units, hotkey);

			if (unitToSelect) {
                if (player.SelectedObject && player.SelectedObject.ObjectId == unitToSelect.ObjectId)
                {
                    MoveCameraToUnit(unitToSelect, camera);
                }
                else
                {
                    ResetPlayerUnitsSelection(units, hud);

                    player.SelectedObject = unitToSelect;
                    unitToSelect.SetSelection(true, hud.GetPlayingArea());
                }
			}
		}

        private static void MoveCameraToUnit (Unit unit, Camera camera)
        {
            camera.transform.position = unit.transform.position + // get camera to unit's position
                Vector3.up * camera.transform.position.y + // lift camera up
                camera.transform.TransformDirection(Vector3.back * 20) +  // pull camera "away" from the unit, in the direction...
                camera.transform.TransformDirection(Vector3.down * 20);   // ...opposite to the one camera is facing atm
        }

        private static void ResetPlayerUnitsSelection (List<Unit> units, HUD hud)
		{
			units.ForEach (p => p.SetSelection (false, hud.GetPlayingArea ()));
		}
	}
}
