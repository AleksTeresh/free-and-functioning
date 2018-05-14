using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class BossPartData : WorldObjectData
    {
        public AbilityAgentData abilityAgent;
        public int aimTargetId;

        public BossPartData(
           WorldObjectData baseObjectData,
           AbilityAgentData abilityAgent,
           int aimTargetId
       ) : base(baseObjectData)
        {
            this.abilityAgent = abilityAgent;
            this.aimTargetId = aimTargetId;
        }
    }
}
