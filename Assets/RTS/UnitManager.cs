using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public static class UnitManager
    {
        public static bool EnemyUnitsExist(Player playerSelf)
        {
            var allPlayers = GameObject.FindObjectsOfType<Player>();

            foreach (var player in allPlayers)
            {
                if (player.username != playerSelf.username && player.GetUnits() != null && player.GetUnits().Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<WorldObject> GetPlayerVisibleEnemies(Player player)
        {
            var unsortedEnemies = player.GetUnits()
                .SelectMany(p =>
                {
                    var stateController = p.GetStateController();
                    var allNearbyEnemies = stateController.nearbyEnemies;

                    return allNearbyEnemies; //.Where(s => s is Unit);
                })
                // .Select(p => (Unit)p)
                .Distinct()
                .ToList();

            unsortedEnemies.Sort((a, b) => a.ObjectId - b.ObjectId);

            return unsortedEnemies;
        }

        public static List<Unit> GetPlayerVisibleMajorEnemies(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(s => s is Unit)
                .Select(p => (Unit)p)
                .Where(p => p.IsMajor())
                .ToList();
        }

        public static List<Unit> GetPlayerVisibleMinorEnemies(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(s => s is Unit)
                .Select(p => (Unit)p)
                .Where(p => !p.IsMajor())
                .ToList();
        }

        public static List<WorldObject> GetVisibleEnemyBossParts(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(s => s is BossPart)
                // .Select(p => (BossPart)p)
                .ToList();
        }

        public static List<WorldObject> GetVisibleEnemyBuildings(Player player)
        {
            return GetPlayerVisibleEnemies(player)
                .Where(s => s is Building)
                // .Select(p => (Building)p)
                .ToList();
        }
    }
}
