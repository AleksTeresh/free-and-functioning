using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class LocalUI : MonoBehaviour {

    private HealthBar healthBar;
    private Canvas localCanvas;

    private WorldObject relatedObj;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        localCanvas = GetComponent<Canvas>();

        relatedObj = GetComponentInParent<WorldObject>();
    }

    private void Update()
    {
        if (relatedObj && relatedObj.GetFogOfWarAgent() && relatedObj.GetFogOfWarAgent().IsObserved())
        {
            localCanvas.enabled = true;

            transform.rotation = Camera.main.transform.rotation;
            transform.position = new Vector3(transform.position.x, relatedObj.GetSelectionBounds().max.y, transform.position.z);

            HandleHealthBar();
        }
        else
        {
            localCanvas.enabled = false;
        }
    }

    private void HandleHealthBar()
    {
        healthBar.SetValue((float)relatedObj.hitPoints / (float)relatedObj.maxHitPoints * 100.0f);
    }

/*
    private void CalculateCurrentHealth()
    {
        healthPercentage = (float)hitPoints / (float)maxHitPoints;
        if (healthPercentage > 0.65f) healthStyle.normal.background = ResourceManager.HealthyTexture;
        else if (healthPercentage > 0.35f) healthStyle.normal.background = ResourceManager.DamagedTexture;
        else healthStyle.normal.background = ResourceManager.CriticalTexture;
    }  */
}
