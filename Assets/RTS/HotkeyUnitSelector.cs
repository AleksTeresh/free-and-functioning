using UnityEngine;

namespace RTS
{
    public static class HotkeyUnitSelector
    {
        private static string[] hotkeys = new string[]
        {
            "Select1",
            "Select2",
            "Select3",
            "Select4",
            "Select5"
        };

        public static void HandleInput(Player player, HUD hud)
        {
            for (int i = 0; i < hotkeys.Length; i++)
            {
                if (Input.GetButtonDown(hotkeys[i]))
                {
                    SelectPlayerUnitByHotkeyIndex(player, hud, i);

                    return;
                }
            }
        }

        private static void SelectPlayerUnitByHotkeyIndex(Player player, HUD hud, int unitIndex)
        {
            Units units = player.GetComponentInChildren<Units>();
            Unit[] playerUnits = units.GetComponentsInChildren<Unit>();

            if (unitIndex < playerUnits.Length)
            {
                ResetPlayerUnitsSelection(playerUnits, hud);

                Unit unitToSelect = playerUnits[unitIndex];

                player.SelectedObject = unitToSelect;
                unitToSelect.SetSelection(true, hud.GetPlayingArea());
            }

        }

        private static void ResetPlayerUnitsSelection(Unit[] units, HUD hud)
        {
            for (int i = 0; i < units.Length; i++)
            {
                units[i].SetSelection(false, hud.GetPlayingArea());
            }
        }
    }
}
