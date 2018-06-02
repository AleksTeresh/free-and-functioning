using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : Indicator
{
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
        if (!player) return;

        if (player.SelectedObject && indicatedObject && player.SelectedObject.ObjectId == indicatedObject.ObjectId)
        {
            mainSelectIndicator.enabled = true;
            subSelectIndicator.enabled = false;
        }
        else if (player.selectedObjects != null && player.selectedObjects.Contains(indicatedObject))
        {
            subSelectIndicator.enabled = true;
            mainSelectIndicator.enabled = false;
        }
        else
        {
            mainSelectIndicator.enabled = false;
            subSelectIndicator.enabled = false;
        }
    }
}
