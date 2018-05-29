﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Statuses;
using RTS;

public class Indicator : MonoBehaviour {
    protected Slider healthSlider;
    protected Text nameLabel;
    // protected Image avatar
    protected Image holdPositionIndicator;
    protected StatusIndicators statusesWrapper;
    protected Image upperSelectIndicator;
    protected Image lowerSelectIndicator;

    protected WorldObject indicatedObject;

    public void Init(WorldObject unit)
    {
        this.indicatedObject = unit;
    }

    public WorldObject GetIndicatedObject()
    {
        return indicatedObject;
    }

    protected virtual void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
        nameLabel = GetComponentInChildren<Text>();
        // avatar = GetComponentInChildren<Image>();
        statusesWrapper = GetComponentInChildren<StatusIndicators>();

        var selectIndicators = new List<SelectHighlight>(GetComponentsInChildren<SelectHighlight>());

        upperSelectIndicator = selectIndicators[0].GetComponent<Image>();
        lowerSelectIndicator = selectIndicators[1].GetComponent<Image>();

        var holdPositionIndicatorSelf = GetComponentInChildren<HoldPositionIndicator>();
        if (holdPositionIndicatorSelf)
        {
            holdPositionIndicator = holdPositionIndicatorSelf.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (indicatedObject)
        {
            healthSlider.maxValue = indicatedObject.maxHitPoints;
            healthSlider.value = indicatedObject.hitPoints;

            nameLabel.text = indicatedObject.name;

            // TODO: uncomment this line once we have actual avatar
            // avatar.sprite = indicatedObject.avatar;
            HandleStatusUpdate();

            HandleSelection();

            if (holdPositionIndicator && indicatedObject is Unit)
            {
                holdPositionIndicator.enabled = ((Unit)indicatedObject).holdingPosition;
            }
        }
    }

    protected virtual void HandleSelection()
    {
        // to be overriden
    }

    private void HandleStatusUpdate()
    {
        var indicators = statusesWrapper.GetComponentsInChildren<StatusIndicator>();
        var indicatedStatuses = indicators.Select(p => p.GetStatus()).ToList();

        int newIndicatorsCounter = 0;

        for (int i = 0; i < indicatedStatuses.Count; i++)
        {
            if (!indicatedStatuses[i] || !indicatedObject.ActiveStatuses.Contains(indicatedStatuses[i]))
            {
                indicatedStatuses = new List<Status>();
                new List<StatusIndicator>(
                    statusesWrapper.transform
                        .GetComponentsInChildren<StatusIndicator>()
                )
                .ForEach(s => Destroy(s.gameObject));
                break;
            }
        }

        indicatedObject.ActiveStatuses.ForEach(p =>
        {
            if (!indicatedStatuses.Contains(p))
            {
                var newIndicatorObject = GameObject.Instantiate(ResourceManager.GetUIElement("StatusIndicator"));
                var newIndicator = newIndicatorObject.GetComponent<StatusIndicator>();

                if (newIndicator)
                {
                    newIndicator.Init(p);
                    newIndicator.transform.SetParent(statusesWrapper.transform, false);

                    var rectTransform = newIndicatorObject.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(
                        rectTransform.anchoredPosition.x - 32 * (indicatedStatuses.Count() + newIndicatorsCounter),
                        rectTransform.anchoredPosition.y
                    );
                    // rectTransform.sizeDelta = new Vector2(0, 100);

                    newIndicatorsCounter++;
                }
            }
        });
    }
}
