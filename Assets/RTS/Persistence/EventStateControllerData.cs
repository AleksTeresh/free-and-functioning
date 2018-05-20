using Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class EventStateControllerData
    {
        public float timeInState;
        public string currentState;
        public DialogManagerData dialogManagerData;
        public List<int> eventObjectIds;
    }
}
