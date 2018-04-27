using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

public class SceneDriver : MonoBehaviour {

    public DialogNode startDialogNode;
    public DialogManager dialogManager;

    private void Start()
    {
        dialogManager.SetDialogNode(startDialogNode);
    }
}
