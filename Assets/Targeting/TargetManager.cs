using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public bool InMultiMode { get; set; }

    public TargetManager ()
    {
        InMultiMode = false;
    }
}
