using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Statuses;
using UnityEngine;
using RTS;
using Persistence;

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

	    public Sprite icon;

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
            cooldownTimer = 0.0f;
        }

        protected void HandleAbilityUseEnd()
        {
            delayTimer = 0.0f;
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

        public AbilityData GetData ()
        {
            var data = new AbilityData();

            data.type = name.Contains("(") ? name.Substring(0, name.IndexOf("(")).Trim() : name;
            data.abilityName = abilityName;
            data.targetIds = targets
                .Select(target => target ? target.ObjectId : -1)
                .Where(id => id != -1)
                .ToList();
            data.isPending = isPending;
            data.blocked = blocked;
            data.cooldownTimer = cooldownTimer;
            data.delayTimer = delayTimer;
            data.statuses = new List<Status>(statuses).Select(status => status.GetData()).ToArray();

            return data;
        }

        public void SetData (AbilityData data)
        {
            abilityName = data.abilityName;
            targets = data.targetIds.Select(id => id != -1 ? Player.GetObjectById(id) : null).ToList();
            isPending = data.isPending;
            blocked = data.blocked;
            cooldownTimer = data.cooldownTimer;
            delayTimer = data.delayTimer;
            statuses = data.statuses.Select(status =>
            {
                var statusObject = (GameObject)GameObject.Instantiate(ResourceManager.GetStatus(status.type));
                var createdStatus = statusObject.GetComponent<Status>();

                return createdStatus;
            }).ToArray();
        }
    }
}