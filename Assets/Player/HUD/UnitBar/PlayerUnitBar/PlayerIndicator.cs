using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : Indicator {

    private Player player;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (unit)
        {
            if (!player)
            {
                player = unit.GetPlayer();
            }

            nameLabel.text = unit.objectName;
        }
    }

    protected override void HandleSelection()
    {
        bool unitIsSelected = player != null &&
            ((
                player.selectedObjects != null &&
                player.selectedObjects.Contains(unit)
            ) ||
            (
                player.SelectedObject && unit &&
                player.SelectedObject.ObjectId == unit.ObjectId
            ));

        upperSelectIndicator.enabled = unitIsSelected;
        lowerSelectIndicator.enabled = unitIsSelected;
    }
}
