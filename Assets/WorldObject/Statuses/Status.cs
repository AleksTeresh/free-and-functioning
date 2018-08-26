using System.Net.Mime;
using Abilities;
using Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace Statuses
{
    public class Status : MonoBehaviour
    {
        public string statusName;
        public float maxDuration;
        public Sprite icon;

        [HideInInspector] public bool isActive;
        [HideInInspector] public WorldObject target;
        [HideInInspector] public float duration;

        protected WorldObject inflictor;
        protected Projectile projectileInflicter;
        protected AreaOfEffect aoeInflicter;

        public void Update()
        {
            if (isActive)
            {

                AffectTarget();

                duration += Time.deltaTime;

                if (duration > maxDuration)
                {
                    FinishStatus();
                }
            }
        }

        public void InflictStatus(WorldObject target, WorldObject inflicter)
        {
            this.inflictor = inflicter;
            InflictStatus(target);
        }

        public void InflictStatus(WorldObject target, Projectile projectileInflicter)
        {
            this.projectileInflicter = projectileInflicter;
            InflictStatus(target);
        }

        public void InflictStatus(WorldObject target, WorldObject inflicter, AreaOfEffect aoeInflicter)
        {
            this.inflictor = inflicter;
            this.aoeInflicter = aoeInflicter;

            InflictStatus(target);
        }

        public void InflictStatus(WorldObject target)
        {
            if (!target) return;

            this.target = target;

            // if the status is not already active on the target, call OnStatusStart
            if (!target.IsStatusActive(this))
            {
                OnStatusStart();
            }

            this.duration = 0;
            this.isActive = true;

            target.AddStatus(this);
        }

        protected virtual void AffectTarget()
        {
            // this method should be overriden
        }

        protected virtual void OnStatusStart()
        {
            // this method should be overriden
        }

        protected virtual void OnStatusEnd()
        {
            // this method should be overriden
        }

        protected void FinishStatus()
        {
            isActive = false;

            OnStatusEnd();
            Destroy(this.gameObject);
        }

        public StatusData GetData ()
        {
            var data = new StatusData();

            data.type = name.Contains("(") ? name.Substring(0, name.IndexOf("(")).Trim() : name;
            data.statusName = statusName;
            data.isActive = isActive;
            data.targetId = target ? target.ObjectId : -1;
            data.duration = duration;

            return data;
        }

        public void SetData (StatusData data)
        {
            statusName = data.statusName;
            isActive = data.isActive;
            target = data.targetId != -1
                ? Player.GetObjectById(data.targetId)
                : null;
            duration = data.duration;
            inflictor = data.inflictorId != -1
                ? Player.GetObjectById(data.inflictorId)
                : null;
        }
    }
}
