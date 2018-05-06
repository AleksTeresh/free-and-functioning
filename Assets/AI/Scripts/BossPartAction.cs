using UnityEngine;
using System.Collections;

namespace AI
{
    public class BossPartAction : Action
    {
        public override void Act(StateController baseController)
        {
            if (!(baseController is BossPartStateController)) return;

            var controller = (BossPartStateController)baseController;

            DoAction(controller);
        }

        protected virtual void DoAction(BossPartStateController controller)
        {
            // the method is to be overriden
        }
    }
}
   
