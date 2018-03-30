using Assets.RTS.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.RTS.Abilities
{
    public class Ability
    {
        public string Name;
        public int Damage;
        public float Cooldown;
        public bool IsHealingAbility;
        public bool InflictsStatus;
        public Status[] Statuses;

        public bool IsReady;

        private float coolDownTimer = 0.0f;

        public void Update() { }
    }
}
