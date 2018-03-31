using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Statuses
{
	public class Status : MonoBehaviour
    {
        public string name;
        public float maxDuration;
        public bool isActive;
		public WorldObject target;
        //public vfx

		public float duration;

        public void Update()
        {
			if (isActive) {

				// Do stuff to unit

				duration += Time.deltaTime;

				if (duration > maxDuration) {
					isActive = false;
				}
			}
        }

		public void InflictStatus(WorldObject target) {
			this.target = target;

			this.duration = 0;
			this.isActive = true;

			// Add status to unit status list
			// Do stuff to unit
		}
    }
}
