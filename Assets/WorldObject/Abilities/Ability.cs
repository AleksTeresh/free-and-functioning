using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Statuses;
using UnityEngine;
using RTS;

namespace Abilities
{
	public class Ability : MonoBehaviour
	{
		public string abilityName;

        public float range;
        public float cooldown;

        public int damage;

        public bool isAllyTargetingAbility = false;
        public bool isSelfOnly = false;

		public Status[] statuses;

		[HideInInspector] public WorldObject user;
        [HideInInspector]  public List<WorldObject> targets;

        [HideInInspector]  public bool isReady = true;

        [HideInInspector]  public float cooldownTimer = 0.0f;

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

		public void Use(WorldObject target)
		{
			if (isReady) {
                this.targets = new List<WorldObject>() { target };

                HandleAbilityUse();

                FireAbility();
            }
            
		}

        public void Use(List<WorldObject> targets)
        {
            if (isReady)
            {
                this.targets = targets;

                HandleAbilityUse();

                FireAbility();
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

        protected void InflictStatuses (WorldObject target)
		{
			for (int i = 0; i < statuses.Length; i++) {
                StatusManager.InflictStatus(user, statuses[i], target);
            }
		}

        protected virtual void OnHit() {
            targets.ForEach(target =>
            {
                InflictStatuses(target);
                target.TakeDamage(damage, AttackType.Ability);
            });

			// Default behaviour needs to be overidden by children
		}
	}
}
