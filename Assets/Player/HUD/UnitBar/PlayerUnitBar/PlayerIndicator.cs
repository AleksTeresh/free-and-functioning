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

        if (indicatedObject)
        {
            if (!player)
            {
                player = indicatedObject.GetPlayer();
            }

            nameLabel.text = indicatedObject.objectName;
        }
    }

    protected override void HandleSelection()
    {
        bool unitIsSelected = player != null &&
            ((
                player.selectedObjects != null &&
                player.selectedObjects.Contains(indicatedObject)
            ) ||
            (
                player.SelectedObject && indicatedObject &&
                player.SelectedObject.ObjectId == indicatedObject.ObjectId
            ));

        upperSelectIndicator.enabled = unitIsSelected;
//        lowerSelectIndicator.enabled = unitIsSelected;
    }
}
