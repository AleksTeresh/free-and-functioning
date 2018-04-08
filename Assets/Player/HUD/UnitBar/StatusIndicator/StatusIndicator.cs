using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Statuses;

public class StatusIndicator : MonoBehaviour {
    protected Text nameLabel;
    protected Image icon;

    protected Status status;

    public void Init(Status status)
    {
        this.status = status;

        // set status' icon as the current icon
        // icon.sprite = status.sprite
        nameLabel.text = status.statusName;
    }

    public Status GetStatus()
    {
        return status;
    }

    void Awake()
    {
        nameLabel = GetComponentInChildren<Text>();
        icon = GetComponentInChildren<Image>();
    }
}
