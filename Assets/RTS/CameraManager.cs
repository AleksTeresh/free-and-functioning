using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public static class CameraManager
    {
        public static void MoveCameraToUnit(Unit unit, Camera camera)
        {
            camera.transform.position = unit.transform.position + // get camera to indicatedObject's position
                Vector3.up * camera.transform.position.y + // lift camera up
                camera.transform.TransformDirection(Vector3.back * 20) +  // pull camera "away" from the indicatedObject, in the direction...
                camera.transform.TransformDirection(Vector3.down * 20);   // ...opposite to the one camera is facing atm
        }

        public static List<Vector3?> CalculateMapFrustum(Camera minimapCamera, Collider mapCollider)
        {
            // _cameraDistance = Camera.main.transform.position.y;
            mapCollider.enabled = true;

            var potentialBottomLeftPos = GetCameraFrustumPosition(new Vector3(0f, 0f), mapCollider);
            Vector3? bottomLeftPos;
            if (potentialBottomLeftPos.HasValue)
            {
                bottomLeftPos = minimapCamera.WorldToViewportPoint(potentialBottomLeftPos.Value);
                bottomLeftPos = new Vector3(bottomLeftPos.Value.x * minimapCamera.orthographicSize * 2, bottomLeftPos.Value.y * minimapCamera.orthographicSize * 2, 1f);
            }
            else
            {
                bottomLeftPos = null;
            }

            var potentialBottomRightPos = GetCameraFrustumPosition(new Vector3(Screen.width, 0f), mapCollider);
            Vector3? bottomRightPos;
            if (potentialBottomRightPos.HasValue)
            {
                bottomRightPos = minimapCamera.WorldToViewportPoint(potentialBottomRightPos.Value);
                bottomRightPos = new Vector3(bottomRightPos.Value.x * minimapCamera.orthographicSize * 2, bottomRightPos.Value.y * minimapCamera.orthographicSize * 2, 1f);
            }
            else
            {
                bottomRightPos = null;
            }

            var potentialTopLeftPos = GetCameraFrustumPosition(new Vector3(0, Screen.height), mapCollider);
            Vector3? topLeftPos;
            if (potentialTopLeftPos.HasValue)
            {
                topLeftPos = minimapCamera.WorldToViewportPoint(potentialTopLeftPos.Value);
                topLeftPos = new Vector3(topLeftPos.Value.x * minimapCamera.orthographicSize * 2, topLeftPos.Value.y * minimapCamera.orthographicSize * 2, 1f);
            }
            else
            {
                topLeftPos = null;
            }

            var potentialTopRightPoss = GetCameraFrustumPosition(new Vector3(Screen.width, Screen.height), mapCollider);
            Vector3? topRightPos;
            if (potentialTopRightPoss.HasValue)
            {
                topRightPos = minimapCamera.WorldToViewportPoint(potentialTopRightPoss.Value);
                topRightPos = new Vector3(topRightPos.Value.x * minimapCamera.orthographicSize * 2, topRightPos.Value.y * minimapCamera.orthographicSize * 2, 1f);
            }
            else
            {
                topRightPos = null;
            }
            
            mapCollider.enabled = false;

            return new List<Vector3?>() { topLeftPos, topRightPos, bottomRightPos, bottomLeftPos };
        }

        private static Vector3? GetCameraFrustumPosition(Vector3 position, Collider mapCollider)
        {
            float biggestDimension = Mathf.Max(mapCollider.transform.localScale.x, mapCollider.transform.localScale.z);

            var cameraDistance = Camera.main.transform.position.y;

            var positionRay = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;

            double maxRaycastDist = Math.Sqrt(cameraDistance * cameraDistance + biggestDimension * biggestDimension) * 10000;

            if (mapCollider.Raycast(positionRay, out hit, Convert.ToSingle(maxRaycastDist)))
            {
                return hit.point;
            }
            else
            {
                return null;
            }
        }
    }
}
