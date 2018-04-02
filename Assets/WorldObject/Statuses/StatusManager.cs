using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;

namespace Statuses
{
    public static class StatusManager
    {
        public static void InflictStatus(WorldObject inflicter, Status status, WorldObject target)
        {
            InflictStatus(inflicter, status, target, target.transform.position, new Quaternion());
        }

        public static void InflictStatus(WorldObject inflicter, Status status, WorldObject target, Vector3 spawnPoint)
        {
            InflictStatus(inflicter, status, target, spawnPoint, new Quaternion());
        }

        public static void InflictStatus(WorldObject inflicter, Status status, WorldObject target, Vector3 spawnPoint, Quaternion rotation)
        {
            // if it is not possible to add the status on the target, skip the rest
            if (!target.CanAddStatus())
            {
                return;
            }

            var newStatusObject = (GameObject)GameObject.Instantiate(ResourceManager.GetStatus(status.name), spawnPoint, rotation);
            var newStatusInstance = newStatusObject.GetComponent<Status>();

            if (newStatusInstance)
            {
                newStatusInstance.InflictStatus(target, inflicter);
            }
        }
    }
}
