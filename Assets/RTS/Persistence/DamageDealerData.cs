using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class DamageDealerData : UnitData
    {
        public int multiDamageMinValue;

        public DamageDealerData (
            UnitData baseData,
            int multiDamageMinValue
        ) : base (baseData)
        {
            this.multiDamageMinValue = multiDamageMinValue;
        }
    }
}
