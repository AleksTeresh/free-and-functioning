using UnityEngine;
using System.Collections;

public class BossPartStateController : StateController
{
    [HideInInspector] public BossPart bossPart;

    protected override void Awake()
    {
        base.Awake();

        bossPart = GetComponent<BossPart>();
    }
}
