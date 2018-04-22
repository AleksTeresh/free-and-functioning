using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class EnemyIndicator : Indicator {

    private TargetManager targetManager;

    protected override void Update ()
    {
        if (!targetManager)
        {
            targetManager = transform.root.GetComponentInChildren<TargetManager>();
        }

        base.Update();
    }

    protected override void HandleSelection()
    {
        bool unitIsSelected = targetManager &&
            targetManager.SingleTarget &&
            targetManager.SingleTarget.ObjectId == indicatedObject.ObjectId;

        upperSelectIndicator.enabled = unitIsSelected;
        lowerSelectIndicator.enabled = unitIsSelected;
    }
}
