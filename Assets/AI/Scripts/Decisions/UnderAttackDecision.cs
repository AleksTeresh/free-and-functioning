using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/UnderAttack")]
public class UnderAttackDecision: Decision
{
    public override bool Decide(StateController controller)
    {
        WorldObject self = controller.controlledObject;

        bool isUnderAttack = self.IsUnderAttack();

        return isUnderAttack;
    }
}