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

		[HideInInspector] public WorldObject user;
        [HideInInspector] public WorldObject target;
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
			if (isReady) {
                this.target = target;

                HandleAbilityUse();

                FireAbility();
            }
            
		}

        public void UseOnTargets(List<WorldObject> targets)
        {
            if (isReady)
            {
                this.targets = targets;

                HandleAbilityUse();

                FireAbilityMulti();
            }
        }

        protected void HandleAbilityUse()
        {
            cooldownTimer = 0.0f;
            isReady = false;
        }

        protected virtual void FireAbility() {
			// Default behaviour needs to be overidden by children
			OnHit();
		}

        protected virtual void FireAbilityMulti()
        {
            // Default behaviour needs to be overidden by children
            OnHitMulti();
        }

        private void InflictStatuses (WorldObject target)
		{
			for (int i = 0; i < statuses.Length; i++) {
                StatusManager.InflictStatus(user, statuses[i], target);
            }
		}

        private void InflictLightStatuses(WorldObject target)
        {
            for (int i = 0; i < lightStatuses.Length; i++)
            {
                StatusManager.InflictStatus(user, lightStatuses[i], target);
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
