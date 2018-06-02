using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class EnemyIndicator : Indicator
{
    private static readonly float SELECTION_OFFSET = 27.0f;

    private TargetManager targetManager;

//    private GameObject background;
    private RectTransform rectTransform;
    private RectTransform backgroundRectTransform;
    private RectTransform healthBarRectTransform;
    private RectTransform selectIndicatorRectTransform;

    protected override void Awake()
    {
        base.Awake();

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        healthBarRectTransform = healthSlider.GetComponent<RectTransform>();
        selectIndicatorRectTransform = mainSelectIndicator.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    protected override void Update()
    {
        if (!targetManager)
        {
            targetManager = transform.root.GetComponentInChildren<TargetManager>();
        }

        base.Update();

        if (indicatedObject)
        {
            nameLabel.text = indicatedObject.objectName;
        }
    }

    protected override void HandleSelection()
    {
        bool unitIsSelected = targetManager &&
                              targetManager.SingleTarget &&
                              targetManager.SingleTarget.ObjectId == indicatedObject.ObjectId;

        mainSelectIndicator.enabled = unitIsSelected;

        DrawBackground(unitIsSelected);
        DrawHealthBar(unitIsSelected);

//        lowerSelectIndicator.enabled = unitIsSelected;
    }

    private void DrawHealthBar(bool unitIsSelected)
    {
        float selectIndicatorWidth = selectIndicatorRectTransform.sizeDelta.x;
        float healthBarOffsetX = unitIsSelected
            ? selectIndicatorWidth - SELECTION_OFFSET
            : selectIndicatorWidth;

        healthBarRectTransform.offsetMax = new Vector2(-healthBarOffsetX, healthBarRectTransform.offsetMax.y);
    }

    private void DrawBackground(bool unitIsSelected)
    {
        float backgroundWidth = unitIsSelected
            ? rectTransform.sizeDelta.x + SELECTION_OFFSET
            : rectTransform.sizeDelta.x;
        float backgroundX = backgroundWidth / 2;

        backgroundRectTransform.sizeDelta = new Vector2(backgroundWidth, backgroundRectTransform.sizeDelta.y);
        backgroundRectTransform.anchoredPosition = new Vector2(backgroundX, backgroundRectTransform.anchoredPosition.y);
    }
}