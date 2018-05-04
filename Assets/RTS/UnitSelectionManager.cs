using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public static class UnitSelectionManager
    {
        public static void HandleUnitSelection (Unit unitToSelect, Player player, Camera camera, HUD hud)
        {
            if (unitToSelect)
            {
                if (
                    (player.selectedObjects.Count == 0 ||
                    (player.selectedObjects.Count == 1 && player.selectedObjects.Contains(unitToSelect))) &&
                    player.SelectedObject &&
                    player.SelectedObject.ObjectId == unitToSelect.ObjectId
                )
                {
                    CameraManager.MoveCameraToUnit(unitToSelect, camera);
                }
                else
                {
                    SelectUnit(player, hud, unitToSelect);
                }
            }
        }

        public static void HandleUnitSelectionWithModifierPress (Unit unitToAdd, Player player, HUD hud)
        {
            if (unitToAdd)
            {
                if (player.SelectedObject == null && player.selectedObjects.Count == 0)
                {
                    SelectUnit(player, hud, unitToAdd);
                }
                else if (!player.selectedObjects.Contains(unitToAdd))
                {
                    AddUnitToSubSelection(player, hud, unitToAdd);
                }
                else
                {
                    RemoveUnitFromSubSelection(player, hud, unitToAdd);
                }
            }
        }


        public static void SelectAllUnits(Player player, HUD hud)
        {
            List<Unit> units = player.GetUnits();

            if (player.SelectedObject)
            {
                foreach (Unit unit in units)
                {
                    if (unit != player.SelectedObject)
                    {
                        AddUnitToSubSelection(player, hud, unit);
                    }
                }
            }
            // if no units are selected, add first from the list as main selection
            else if (units.Count > 0)
            {
                SelectUnit(player, hud, units[0]);

                for (int i = 1; i < units.Count; i++)
                {
                    AddUnitToSubSelection(player, hud, units[i]);
                }
            }
        }

        private static void SelectUnit(Player player, HUD hud, Unit unitToSelect)
        {
            ResetPlayerUnitsSelection(player, hud);

            player.SelectedObject = unitToSelect;

            if (!player.selectedObjects.Contains(unitToSelect))
            {
                player.selectedObjects.Add(unitToSelect);
            }

            unitToSelect.SetSelection(true, hud.GetPlayingArea());
        }

        private static void AddUnitToSubSelection(Player player, HUD hud, Unit unitToAdd)
        {
            List<WorldObject> selectedObjects = player.selectedObjects;

            if (unitToAdd && !selectedObjects.Contains(unitToAdd))
            {
                unitToAdd.SetSelection(true, hud.GetPlayingArea());

                selectedObjects.Add(unitToAdd);
            }
        }

        private static void RemoveUnitFromSubSelection(Player player, HUD hud, Unit unitToRemove)
        {
            List<WorldObject> selectedObjects = player.selectedObjects;

            if (unitToRemove && selectedObjects.Contains(unitToRemove))
            {
                unitToRemove.SetSelection(false, hud.GetPlayingArea());
                if (player.SelectedObject && player.SelectedObject.ObjectId == unitToRemove.ObjectId)
                {
                    player.SelectedObject = null;
                }

                selectedObjects.Remove(unitToRemove);
            }
        }

        private static void ResetPlayerUnitsSelection(Player player, HUD hud)
        {
            player.SelectedObject = null;
            List<Unit> units = player.GetUnits();

            units.ForEach(p => p.SetSelection(false, hud.GetPlayingArea()));
            player.selectedObjects.Clear();
        }
    }
}
