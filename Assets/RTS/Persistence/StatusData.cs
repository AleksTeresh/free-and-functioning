using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class StatusData
    {
        public string type;
        public string statusName;
        public bool isActive;
        public int targetId;
        public float duration;
        public int inflictorId;
    }
}
