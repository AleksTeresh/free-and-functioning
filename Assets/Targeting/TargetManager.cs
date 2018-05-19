using Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public bool InMultiMode { get; set; }
    public WorldObject SingleTarget { get; set; }

    public TargetManager ()
    {
        InMultiMode = false;
        SingleTarget = null;
    }

    private void Update()
    {
        if (
            SingleTarget &&
            SingleTarget is Unit &&
            SingleTarget.GetFogOfWarAgent() &&
            !SingleTarget.GetFogOfWarAgent().IsObserved()
        )
        {
            SingleTarget = null;
        }
    }

    public TargetManagerData GetData ()
    {
        var data = new TargetManagerData();

        data.singleTargetId = SingleTarget
            ? SingleTarget.ObjectId
            : -1;
        data.inMultiMode = InMultiMode;

        return data;
    }

    public void SetData (TargetManagerData data)
    {
        InMultiMode = data.inMultiMode;
        SingleTarget = data.singleTargetId != -1
            ? Player.GetObjectById(data.singleTargetId)
            : null;
    }
}
