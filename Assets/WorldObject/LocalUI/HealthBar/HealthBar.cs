using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public float GetValue()
    {
        return slider.value;
    }

    public void SetEnable(bool enable)
    {
        slider.enabled = enable;
    }

    public void SetValue(float value)
    {
        // TODO: optioanlly add color changing here

        slider.value = value;
    }
}
