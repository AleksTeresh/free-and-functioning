﻿using System.Collections;
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
}
