using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Statuses
{
	public class Status : MonoBehaviour
    {
        public string statusName;
        public float maxDuration;
        public bool isActive;
		public WorldObject target;
        //public vfx

        protected WorldObject inflicter;

		public float duration;

        public void Update()
        {
			if (isActive) {

                AffectTarget();

                duration += Time.deltaTime;

				if (duration > maxDuration) {
					isActive = false;

                    OnStatusEnd();
                    Destroy(this.gameObject);
				}
			}
        }

		public void InflictStatus(WorldObject target, WorldObject inflicter) {
			this.target = target;
            this.inflicter = inflicter;

			this.duration = 0;
			this.isActive = true;

            target.AddStatus(this);
		}

        protected virtual void AffectTarget()
        {
            // this method should be overriden
        }

        protected virtual void OnStatusEnd()
        {
            // this method should be overriden
        }
    }
}
