using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Abilities;

namespace RTS
{
    public static class WorkManager
    {
        public static Rect CalculateSelectionBox(Bounds selectionBounds, Rect playingArea)
        {
            //shorthand for the coordinates of the centre of the selection bounds
            float cx = selectionBounds.center.x;
            float cy = selectionBounds.center.y;
            float cz = selectionBounds.center.z;
            //shorthand for the coordinates of the extents of the selection bounds
            float ex = selectionBounds.extents.x;
            float ey = selectionBounds.extents.y;
            float ez = selectionBounds.extents.z;

            //Determine the screen coordinates for the corners of the selection bounds
            List<Vector3> corners = new List<Vector3>();
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz + ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy + ey, cz - ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz + ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz + ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx + ex, cy - ey, cz - ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz + ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy + ey, cz - ez)));
            corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx - ex, cy - ey, cz - ez)));

            //Determine the bounds on screen for the selection bounds
            Bounds screenBounds = new Bounds(corners[0], Vector3.zero);
            for (int i = 1; i < corners.Count; i++)
            {
                screenBounds.Encapsulate(corners[i]);
            }

            //Screen coordinates start in the bottom left corner, rather than the top left corner
            //this correction is needed to make sure the selection box is drawn in the correct place
            float selectBoxTop = playingArea.height - (screenBounds.center.y + screenBounds.extents.y);
            float selectBoxLeft = screenBounds.center.x - screenBounds.extents.x;
            float selectBoxWidth = 2 * screenBounds.extents.x;
            float selectBoxHeight = 2 * screenBounds.extents.y;

            return new Rect(selectBoxLeft, selectBoxTop, selectBoxWidth, selectBoxHeight);
        }

        public static bool ObjectIsGround(GameObject obj)
        {
            return obj.name == "Ground" || obj.name == "Ground(Clone)";
        }

        public static List<WorldObject> FindNearbyObjects(Vector3 position, float range)
        {
            Collider[] hitColliders = Physics.OverlapSphere(position, range);
            HashSet<int> nearbyObjectIds = new HashSet<int>();
            List<WorldObject> nearbyObjects = new List<WorldObject>();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Transform parent = hitColliders[i].transform.parent;
                if (parent)
                {
                    WorldObject parentObject = parent.GetComponent<WorldObject>();

                    if (
                        parentObject && !nearbyObjectIds.Contains(parentObject.ObjectId)
                    )
                    {
                        nearbyObjectIds.Add(parentObject.ObjectId);
                        nearbyObjects.Add(parentObject);
                    }
                }
            }

            nearbyObjects.Sort((a, b) => a.ObjectId - b.ObjectId);
            return nearbyObjects;
        }

        public static WorldObject FindNearestWorldObjectInListToPosition(List<WorldObject> objects, Vector3 position)
        {
            if (objects == null || objects.Count == 0) return null;
            WorldObject nearestObject = objects[0];
            float distanceToNearestObject = Vector3.Distance(position, nearestObject.transform.position);
            for (int i = 1; i < objects.Count; i++)
            {
                float distanceToObject = Vector3.Distance(position, objects[i].transform.position);
                if (distanceToObject < distanceToNearestObject)
                {
                    distanceToNearestObject = distanceToObject;
                    nearestObject = objects[i];
                }
            }
            return nearestObject;
        }

        public static WorldObject FindMostVulnerableObjectInList(List<WorldObject> objects)
        {
            if (objects == null || objects.Count == 0) return null;
            WorldObject mostVulnerable = objects[0];

            objects.ForEach(p =>
            {
                if (p.maxHitPoints < mostVulnerable.maxHitPoints)
                {
                    mostVulnerable = p;
                }
            });

            return mostVulnerable;
        }

        public static WorldObject FindMostDamagingObjectInList(List<WorldObject> objects)
        {
            if (objects == null || objects.Count == 0) return null;
            WorldObject mostDamaging = objects[0];

            objects.ForEach(p =>
            {
                if (p.maxHitPoints < mostDamaging.damage)
                {
                    mostDamaging = p;
                }
            });

            return mostDamaging;
        }

        public static List<WorldObject> FindReachableObjects(List<WorldObject> objects, Vector3 currentPosition, float range)
        {
            List<WorldObject> reachable = objects
                .Where(p =>
                {
                    Vector3 currentObjPosition = p.transform.position;
                    Vector3 direction = currentObjPosition - currentPosition;

                    return direction.sqrMagnitude < range * range;
                })
                .ToList();

            return reachable;
        }

        public static List<WorldObject> FindMeleeObjectsInList (List<WorldObject> objects)
        {
            var meleeUnits = objects.Where(p => p is MeleeUnit).ToList();

            return meleeUnits;
        }

        public static MeleeUnit FindNearestMeleeObject (List<WorldObject> objects, Vector3 position)
        {
            if (objects == null || objects.Count == 0) return null;

            WorldObject nearestObject = null;
            float distanceToNearestObject = -1;

            foreach (var obj in objects)
            {
                if (obj is MeleeUnit)
                {
                    float distanceToObject = Vector3.Distance(position, obj.transform.position);
                    if (distanceToObject < distanceToNearestObject || distanceToNearestObject < 0)
                    {
                        distanceToNearestObject = distanceToObject;
                        nearestObject = obj;
                    }
                }
            }

            return nearestObject == null
                ? null
                : (MeleeUnit) nearestObject;
        }

        public static List<WorldObject> GetEnemyObjects (List<WorldObject> objects, Player ownPlayer)
        {
            return objects
                .Where(p =>
                    p.GetPlayer() != null &&
                    p.GetPlayer() != ownPlayer &&
                    p.GetFogOfWarAgent() &&
                    p.GetFogOfWarAgent().IsObserved()
                ) // do not attack friendly units or neutral objects
                .ToList();
        }

        public static List<WorldObject> GetAllyObjects(List<WorldObject> objects, Player ownPlayer)
        {
            return objects
                .Where(p => p.GetPlayer() != null && p.GetPlayer() == ownPlayer)
                .ToList();
        }

        public static Vector3 GetPerpendicularDestinationPoint (NavMeshAgent agent, Vector3 currentDestination, float walkRadius)
        {
            var perpendicularDirection = Vector3.Cross(currentDestination - agent.transform.position, Vector3.up);

            return agent.transform.position + perpendicularDirection.normalized * walkRadius;
        }

        public static Vector3 GetRandomDestinationPoint(Vector3 center, float walkRadius)
        {
            Vector2 randomPointOnCircle = Random.insideUnitCircle * walkRadius;
            Vector3 randomPoint = new Vector3(randomPointOnCircle.x, 0, randomPointOnCircle.y) + center;

            return GetClosestPointOnNavMesh(randomPoint, "Walkable", walkRadius);
        }

        public static Vector3 GetClosestPointOnNavMesh(Vector3 initialPoint, string areaName, float walkRadius)
        {
            NavMeshHit hit;
            
            bool result = NavMesh.SamplePosition(initialPoint, out hit, walkRadius, GetNavMeshAreaFromName(areaName));
            Vector3 finalPosition = hit.position;

            return finalPosition;
        }

        public static int GetNavMeshAreaFromName (string areaName)
        {
            var areaMaskFromName = 1 << NavMesh.GetAreaFromName(areaName);

            return areaMaskFromName;
        }

        public static bool ObjectCanReachTarget(WorldObject self, WorldObject target)
        {
            Vector3 targetLocation = WorkManager.GetTargetClosestPoint(self, target);
            Vector3 direction = targetLocation - self.transform.position;
            float targetDistance = direction.magnitude;

            return direction.magnitude <= (0.8f * self.weaponRange) && target.GetFogOfWarAgent().IsObserved();
        }

		public static bool ObjectCanReachTargetWithAbility(WorldObject self, Ability ability, WorldObject target)
		{
			if (!ability) 
			{
				return false;
			}

			Vector3 targetLocation = WorkManager.GetTargetClosestPoint(self, target);
			Vector3 direction = targetLocation - self.transform.position;
			float targetDistance = direction.magnitude;

			return direction.magnitude <= (0.8f * ability.range) && target.GetFogOfWarAgent().IsObserved();
		}

        public static bool IsObjectFacingTarget (WorldObject obj, WorldObject target)
        {
            Vector3 targetLocation = target.transform.position;
            Vector3 direction = targetLocation - obj.transform.position;

            // ignore height when considering 
            var a = new Vector3(direction.normalized.x, 0, direction.normalized.z);
            var b = new Vector3(obj.transform.forward.normalized.x, 0, obj.transform.forward.normalized.z);

            return V3Equal(a, b);
        }

        public static bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.1;
        }

        public static int GetTargetSelectionIndex (WorldObject currentTarget, List<Unit> majorEnemies)
        {
            if (!currentTarget || majorEnemies.Count == 0)
            {
                return -1;
            }

            for (int i = 0; i < majorEnemies.Count; i++)
            {
                if (currentTarget.ObjectId == majorEnemies[i].ObjectId)
                {
                    return i;
                }
            }

            return -1;
        }

        public static Vector3 GetTargetClosestPoint(WorldObject attacker, WorldObject target)
        {
            return target.GetSelectionBounds().ClosestPoint(attacker.transform.position);
        }
    }
}
