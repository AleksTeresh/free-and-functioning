using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RTS;
using Abilities;

namespace Statuses
{
    public static class StatusManager
    {
        public static void InflictStatus(WorldObject inflicter, Status status, WorldObject target)
        {
            InflictStatus(inflicter, status, target, target.transform.position, new Quaternion());
        }

        public static void InflictStatus(Projectile inflicter, Status status, WorldObject target)
        {
            InflictStatus(inflicter, status, target, target.transform.position, new Quaternion());
        }

        public static void InflictStatus(WorldObject inflicter, AreaOfEffect aoeInflicter, Status status, WorldObject target)
        {
            InflictStatus(inflicter, aoeInflicter, status, target, target.transform.position, new Quaternion());
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

            Status newStatusInstance = InstantiateStatus(status, spawnPoint, rotation);

            if (newStatusInstance)
            {
                newStatusInstance.InflictStatus(target, inflicter);
            }
        }

        public static void InflictStatus(WorldObject inflicter, AreaOfEffect aoeInflicter, Status status, WorldObject target, Vector3 spawnPoint, Quaternion rotation)
        {
            // if it is not possible to add the status on the target, skip the rest
            if (!target.CanAddStatus())
            {
                return;
            }

            Status newStatusInstance = InstantiateStatus(status, spawnPoint, rotation);

            if (newStatusInstance)
            {
                newStatusInstance.InflictStatus(target, inflicter, aoeInflicter);
            }
        }

        public static void InflictStatus(Projectile inflicter, Status status, WorldObject target, Vector3 spawnPoint, Quaternion rotation)
        {
            // if it is not possible to add the status on the target, skip the rest
            if (!target.CanAddStatus())
            {
                return;
            }

            Status newStatusInstance = InstantiateStatus(status, spawnPoint, rotation);

            if (newStatusInstance)
            {
                newStatusInstance.InflictStatus(target, inflicter);
            }
        }

        private static Status InstantiateStatus(Status status, Vector3 spawnPoint, Quaternion rotation)
        {
            var newStatusObject = (GameObject)GameObject.Instantiate(ResourceManager.GetStatus(status.name), spawnPoint, rotation);
            return newStatusObject.GetComponent<Status>();
        }
    }
}
