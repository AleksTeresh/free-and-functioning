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
        public float delayTime;

        public int damage;
        public int multiDamageMinValue = 1;
		public AttackType attackType = AttackType.Ability;

        public bool isAllyTargetingAbility = false;
        public bool isSelfOnly = false;

		public Status[] statuses;

		[HideInInspector] public WorldObject user;
        [HideInInspector]  public List<WorldObject> targets;

        [HideInInspector] public bool isPending = false;
        [HideInInspector] public bool blocked = false;

        [HideInInspector]  public float cooldownTimer = 0.0f;
        [HideInInspector] public float delayTimer = 0.0f;

        public void Awake()
        {
            user = GetComponentInParent<WorldObject>();
            cooldownTimer = cooldown;
        }

        public void Update ()
		{ 
			if (!IsReady()) {
				cooldownTimer += Time.deltaTime;
			}

            if (isPending)
            {
                delayTimer += Time.deltaTime;

                if (delayTimer >= delayTime)
                {
                    HandleAbilityUseEnd();

                    FireAbility();
                }
            }
		}

		public void Use(WorldObject target)
		{
			if (IsReady()) {
                this.targets = new List<WorldObject>() { target };

                HandleAbilityUseStart();
            }
		}

        public void Use(List<WorldObject> targets)
        {
            if (IsReady())
            {
                this.targets = targets;

                HandleAbilityUseStart();
            }
        }

        public bool IsReady()
        {
            return cooldownTimer >= cooldown && !blocked;
        }

        protected void HandleAbilityUseStart()
        {
            // TODO: add animation handling

            isPending = true;
            // isReady = false;
            cooldownTimer = 0.0f;
        }

        protected void HandleAbilityUseEnd()
        {
            delayTimer = 0.0f;
            // isReady = false;
            isPending = false;
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
            int dividedDamage = Mathf.Max(multiDamageMinValue, damage / targets.Count);
            targets.ForEach(target =>
            {
                if (!target) return;

                InflictStatuses(target);
                target.TakeDamage(dividedDamage, AttackType.Ability);
            });

			// Default behaviour needs to be overidden by children
		}
	}
}
