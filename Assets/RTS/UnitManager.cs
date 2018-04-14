using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS
{
    public static class UnitManager
    {
        public static List<Unit> GetPlayerVisibleEnemies(Player player)
        {
            return player.GetUnits()
                .SelectMany(p => p.GetStateController().nearbyEnemies.Where(s => s is Unit))
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
    }
}
