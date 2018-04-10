using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public static class AoeUtils
    {
        public static Vector3 GetAoeTargetPosition (float radius, List<WorldObject> potentialTargets, Player ownPlayer)
        {
            WorldObject bestTarget = null;
            int bestEnemyCount = 0;

            potentialTargets.ForEach(p =>
            {
                if (!p) return;

                var reachableObjects = WorkManager.FindNearbyObjects(p.transform.position, radius)
                    .Where(s => s.GetPlayer() != null && s.GetPlayer() != ownPlayer); // do not include friendly units or neutral objects;

                if (reachableObjects.Count() > bestEnemyCount)
                {
                    bestEnemyCount = reachableObjects.Count();
                    bestTarget = p;
                }
            });

            if (bestTarget)
            {
                return bestTarget.transform.position;
            }

            return new Vector3();
        }
    }
}
