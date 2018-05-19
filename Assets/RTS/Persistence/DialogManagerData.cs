using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    [Serializable]
    public class DialogManagerData
    {
        public Queue<string> sentences;
        public string currentDialogNode;
        public bool blockGameplay;
    }
}
