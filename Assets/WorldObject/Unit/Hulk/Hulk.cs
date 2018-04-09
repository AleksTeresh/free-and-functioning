using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hulk : MeleeUnit {

    public override bool CanAttack()
    {
        return true;
    }

    public override bool IsMajor()
    {
        return true;
    }
}
