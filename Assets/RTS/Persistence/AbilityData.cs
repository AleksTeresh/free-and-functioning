using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class AbilityData
    {
        public string type;
        public string abilityName;
        public StatusData[] statuses;
        public List<int> targetIds;
        public bool isPending;
        public bool blocked;
        public float cooldownTimer;
        public float delayTimer;
    }
}
