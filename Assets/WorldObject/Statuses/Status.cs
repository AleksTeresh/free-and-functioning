using Abilities;
using UnityEngine;

namespace Statuses
{
    public class Status : MonoBehaviour
    {
        public string statusName;
        public float maxDuration;

        [HideInInspector] public bool isActive;
        [HideInInspector] public WorldObject target;
        [HideInInspector] public float duration;
        //public vfx

        protected WorldObject inflicter;
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
            this.inflicter = inflicter;
            InflictStatus(target);
        }

        public void InflictStatus(WorldObject target, Projectile projectileInflicter)
        {
            this.projectileInflicter = projectileInflicter;
            InflictStatus(target);
        }

        public void InflictStatus(WorldObject target, WorldObject inflicter, AreaOfEffect aoeInflicter)
        {
            this.inflicter = inflicter;
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
    }
}
