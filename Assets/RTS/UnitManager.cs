using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public static class UnitManager
    {
        public static List<Unit> GetPlayerVisibleEnemies(Player player)
        {
            return player.GetUnits()
                .SelectMany(p =>
                {
                    var stateController = p.GetStateController();
                    var allNearbyEnemies = stateController.nearbyEnemies;

                    return allNearbyEnemies.Where(s => s is Unit);
                })
                .Select(p => (Unit)p)
                .Distinct()
                .ToList();
        }

        public static List<Unit> GetPlayerVisibleMajorEnemies(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(p => p.IsMajor())
                .ToList();
        }

        public static List<Unit> GetPlayerVisibleMinorEnemies(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(p => !p.IsMajor())
                .ToList();
        }

        public static Vector3 CalculateFormationOffset(Player player, Unit unit)
        {
            var leader = GetLeader(player.selectedObjects);

            if (leader)
            {
                return unit.transform.position - leader.transform.position;
            }

            return Vector3.zero;
        }

        public static WorldObject GetLeader (List<WorldObject> selectedObjects_)
        {
            var selectedObjects = selectedObjects_.Where(p => p is Unit).ToList();
            if (selectedObjects.Count == 0) return null;

            int maxHealth = selectedObjects.Max(p => p.maxHitPoints);

            return selectedObjects.FindLast(p => p.maxHitPoints == maxHealth);
        }
    }
}
