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

        public float range;
        public float cooldown;
        public bool isHealingAbility;

        public bool isMultiTarget = true;

        public int damage;
        public int lightDamage;

		public Status[] statuses;
        public Status[] lightStatuses;

		public WorldObject user;
		public WorldObject target;
        public List<WorldObject> targets;

		public bool isReady = true;

		public float cooldownTimer = 0.0f;

        public void Awake()
        {
            user = GetComponentInParent<WorldObject>();
            cooldownTimer = cooldown;
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

        public void UseOnTargets(List<WorldObject> targets)
        {
            this.targets = targets;

            if (isReady)
            {
                cooldownTimer = 0.0f;
                isReady = false;
            }

            FireAbilityMulti();
        }

		public virtual void FireAbility() {
			// Default behaviour needs to be overidden by children
			OnHit();
		}

        public virtual void FireAbilityMulti()
        {
            // Default behaviour needs to be overidden by children
            OnHitMulti();
        }

		private void InflictStatuses (WorldObject target)
		{
			for (int i = 0; i < statuses.Length; i++) {
				statuses [i].InflictStatus (target);
			}
		}

        private void InflictLightStatuses(WorldObject target)
        {
            for (int i = 0; i < lightStatuses.Length; i++)
            {
                statuses[i].InflictStatus(target);
            }
        }

        protected virtual void OnHit() {
			InflictStatuses(target);
			target.TakeDamage (damage);

			// Default behaviour needs to be overidden by children
		}

        protected virtual void OnHitMulti()
        {
            targets.ForEach(target =>
            {
                InflictLightStatuses(target);
                target.TakeDamage(lightDamage);
            });
        }
	}
}
