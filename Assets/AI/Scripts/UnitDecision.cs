using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDecision : Decision
{
    public override bool Decide(StateController baseController)
    {
        if (!(baseController is UnitStateController)) return false;

        var controller = (UnitStateController)baseController;

        return DoDecide (controller);
    }

    protected abstract bool DoDecide(UnitStateController controller);
}
