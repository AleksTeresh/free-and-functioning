using UnityEngine;

public class BossPartStateController : StateController
{
    [HideInInspector] public BossPart bossPart;

    protected override void AwakeObj()
    {
        base.AwakeObj();

        bossPart = GetComponent<BossPart>();
    }
}
