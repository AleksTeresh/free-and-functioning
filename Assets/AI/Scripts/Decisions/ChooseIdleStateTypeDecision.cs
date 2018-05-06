using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateAssetMenu(menuName = "AI/Decisions/ChooseIdleStateType")]
    public class ChooseIdleStateTypeDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            bool canAttackMulti = controller.controlledObject.CanAttackMulti();
            bool isInMultiMode = controller.targetManager.InMultiMode;

            return isInMultiMode;
            // return canAttackMulti ? isInMultiMode : false;
        }
    }
}
