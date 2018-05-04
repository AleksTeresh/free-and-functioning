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
    }
}
