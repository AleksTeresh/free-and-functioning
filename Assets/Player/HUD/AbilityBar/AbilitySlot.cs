using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour {

    public Image Image { get; private set; }
    public Text Name { get; private set; }
    public RectTransform Shade { get; private set; }

    void Awake()
    {
        Image = GetComponentInChildren<Image>();
        Name = GetComponentInChildren<AbilityNameLabel>().GetComponent<Text>();
        Shade = GetComponentInChildren<AbilityShade>().GetComponent<RectTransform>();
    }
}
