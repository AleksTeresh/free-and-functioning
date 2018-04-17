using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Formation
{
    public class FormationUtils
    {
        public static Vector3 CalculateManualFormationOffset(Player player, Unit unit)
        {
            var leader = GetLeaderForManualFormation(player.selectedObjects);

            if (leader)
            {
                return unit.transform.position - leader.transform.position;
            }

            return Vector3.zero;
        }

        public static Vector3 CalculateAutoFormationOffset(Player player, Unit unit, Vector3 destination)
        {
            var leader = GetLeaderForAutoFormation(player.selectedObjects);

            if (leader == null)
            {
                return Vector3.zero;
            }

            var goDirection = destination - leader.transform.position;
            var perpendicularDirection = Vector3.Cross(goDirection, Vector3.up);

            if (unit is Tanker || leader.ObjectId == unit.ObjectId)
            {
                return Vector3.zero;
            }
            else if (unit is DamageDealer)
            {
                return -goDirection.normalized * 10;
            }
            else if (unit is Healer)
            {
                return -goDirection.normalized * 20 + perpendicularDirection.normalized * 10;
            }
            else if (unit is CrowdControl)
            {
                return -goDirection.normalized * 20 - perpendicularDirection.normalized * 10;
            }

            return Vector3.zero;
        }

        public static WorldObject GetLeaderForManualFormation(List<WorldObject> selectedObjects_)
        {
            var selectedObjects = selectedObjects_.Where(p => p is Unit).ToList();
            if (selectedObjects.Count == 0) return null;

            int maxHealth = selectedObjects.Max(p => p.maxHitPoints);

            return selectedObjects.FindLast(p => p.maxHitPoints == maxHealth);
        }

        public static WorldObject GetLeaderForAutoFormation(List<WorldObject> selectedObjects_)
        {
            var selectedObjects = selectedObjects_.Where(p => p is Unit).ToList();
            if (selectedObjects.Count == 0) return null;

            if (selectedObjects.Exists(p => p is Tanker))
            {
                return selectedObjects.Find(p => p is Tanker);
            }
            else if (selectedObjects.Exists(p => p is DamageDealer))
            {
                return selectedObjects.Find(p => p is DamageDealer);
            }
            else if (selectedObjects.Exists(p => p is CrowdControl))
            {
                return selectedObjects.Find(p => p is CrowdControl);
            }
            if (selectedObjects.Exists(p => p is Healer))
            {
                return selectedObjects.Find(p => p is Healer);
            }

            return null;
        }
    }
}
