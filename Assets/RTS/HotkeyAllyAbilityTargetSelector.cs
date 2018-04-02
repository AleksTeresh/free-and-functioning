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

		public static void HandleInput (Player player, HUD hud)
		{
			if (player.selectedAllyTargettingAbility != null) 
			{
				for (int i = 0; i < hotkeys.Length; i++) 
				{
					if (Input.GetButtonDown (hotkeys [i])) 
					{
						UseAbilityOnAlly (player, i);

						return;
					}
				}
			}
		}

		private static void UseAbilityOnAlly (Player player, int unitIndex)
		{
			Unit allyTarget = player.FindUnitByIdx (unitIndex);

			if (allyTarget != null) 
			{
				InputToCommandManager.AllyAbilityTargetSelectionToState (
					player.SelectedObject.GetStateController(), player.selectedAllyTargettingAbility, allyTarget);
				player.selectedAllyTargettingAbility = null;
			}
		}
	}
}
