using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Statuses;
using UnityEngine;

namespace Abilities
{
	public class Ability : MonoBehaviour
	{
		public string abilityName;
		public int damage;
		public float cooldown;
		public bool isHealingAbility;
		public Status[] statuses;
		public float range;
		public WorldObject user;
		public WorldObject target;

		public bool isReady = true;

		public float cooldownTimer = 0.0f;

        public void Awake()
        {
            user = GetComponentInParent<WorldObject>();
        }

        public void Update ()
		{ 
			if (!isReady) {
				cooldownTimer += Time.deltaTime;

				if (cooldownTimer >= cooldown) {
					isReady = true;
				}
			}
		}

		public void UseOnTarget (WorldObject target)
		{
			this.target = target;

			if (isReady) {
				cooldownTimer = 0.0f;

				isReady = false;
			}

			FireAbility ();
		}

		public virtual void FireAbility() {
			// Default behaviour needs to be overidden by children
			OnHit();
		}

		private void InflictStatuses (WorldObject target)
		{
			for (int i = 0; i < statuses.Length; i++) {
				statuses [i].InflictStatus (target);
			}
		}

		protected virtual void OnHit() {
			InflictStatuses(target);
			target.TakeDamage (damage);

			// Default behaviour needs to be overidden by children
		}
	}
}
