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

						if (Input.GetButton ("SelectionModifier"))
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

				if (Input.GetButtonDown("SelectAll") )
				{
					SelectAllUnits (player, hud);
				}
			}
		}

		private static void HandleUnitHotkeyPress (Camera camera, Player player, HUD hud, int hotkey)
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
					SelectUnit (player, hud, unitToSelect);
                }
			}
		}

		private static void SelectUnit(Player player, HUD hud, Unit unitToSelect) 
		{
			ResetPlayerUnitsSelection(player, hud);

			player.SelectedObject = unitToSelect;
			unitToSelect.SetSelection(true, hud.GetPlayingArea());
		}

        private static void MoveCameraToUnit (Unit unit, Camera camera)
        {
            camera.transform.position = unit.transform.position + // get camera to unit's position
                Vector3.up * camera.transform.position.y + // lift camera up
                camera.transform.TransformDirection(Vector3.back * 20) +  // pull camera "away" from the unit, in the direction...
                camera.transform.TransformDirection(Vector3.down * 20);   // ...opposite to the one camera is facing atm
        }



		private static void HandleUnitHotkeyWithModifierPress (Player player, HUD hud, int hotkey)
		{
			Unit unitToAdd = player.unitMapping.FindUnitByHotkey (player.GetUnits(), hotkey);

			if (unitToAdd) 
			{
				if (player.SelectedObject == null) 
				{
					SelectUnit (player, hud, unitToAdd);
				}
				else 
				{
					AddUnitToSubSelection (player, hud, unitToAdd);
				}
			}


		}

		private static void AddUnitToSubSelection(Player player, HUD hud, Unit unitToAdd)
		{
			List<WorldObject> selectedObjects = player.selectedObjects;

			if (unitToAdd && !selectedObjects.Contains (unitToAdd)) 
			{
				unitToAdd.SetSelection (true, hud.GetPlayingArea ());

				selectedObjects.Add (unitToAdd);
			}
		}

		private static void SelectAllUnits(Player player, HUD hud)
		{
			List<Unit> units = player.GetUnits ();

			if (player.SelectedObject) 
			{
				foreach (Unit unit in units) 
				{
					if (unit != player.SelectedObject) 
					{
						AddUnitToSubSelection (player, hud, unit);
					}
				}
			}
			// if no units are selected, add first from the list as main selection
			else if (units.Count > 0)
			{
				SelectUnit (player, hud, units [0]);

				for (int i = 1; i < units.Count; i++)
				{
					AddUnitToSubSelection (player, hud, units [i]);
				}
			}
		}

		private static void ResetPlayerUnitsSelection (Player player, HUD hud)
		{
			player.SelectedObject = null;
			List<Unit> units = player.GetUnits ();

			units.ForEach (p => p.SetSelection (false, hud.GetPlayingArea ()));
			player.selectedObjects.Clear ();
		}
	}
}
