using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
                        parentObject && !nearbyObjectIds.Contains(parentObject.ObjectId) &&
                        parentObject.GetFogOfWarAgent() && parentObject.GetFogOfWarAgent().IsObserved()
                    )
                    {
                        nearbyObjectIds.Add(parentObject.ObjectId);
                        nearbyObjects.Add(parentObject);
                    }
                }
            }
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

        public static bool ObjectCanReachTarget(WorldObject self, FogOfWarAgent target)
        {
            Vector3 targetLocation = target.transform.position;
            Vector3 direction = targetLocation - self.transform.position;
            float targetDistance = direction.magnitude;

            return direction.magnitude <= (0.8f * self.weaponRange) && target.IsObserved();
        }
/*
        public static Vector3 FindNearestAttackPosition(NavMeshAgent selfAgent, WorldObject self, WorldObject target)
        {
            if (target && target.GetFogOfWarAgent() && target.GetFogOfWarAgent().IsObserved())
            {
                return FindNearestAttackPositionAlongStraightLine(
                    self.weaponRange,
                    self.transform.position,
                    target.transform.position
                );
            }
            else
            {
                var navMeshPath = new NavMeshPath();
                bool result = selfAgent.CalculatePath(target.transform.position, navMeshPath);

                if (result && navMeshPath.corners.Length > 1)
                {
                    return FindNearestAttackPositionAlongStraightLine(
                        self.weaponRange,
                        navMeshPath.corners[navMeshPath.corners.Length - 2],
                        target.transform.position
                    );
                }

                return FindNearestAttackPositionAlongStraightLine(
                    self.weaponRange,
                    self.transform.position,
                    target.transform.position
                );
            }
        }
*/

        public static bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.001;
        }
    }
}
