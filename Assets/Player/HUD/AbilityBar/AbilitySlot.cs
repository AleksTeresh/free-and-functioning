using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour {

    public Image Frame { get; private set; }
    public Image Image { get; private set; }
    public Text Name { get; private set; }
    public RectTransform Shade { get; private set; }

    void Awake()
    {
        var allImages = new List<Image>(GetComponentsInChildren<Image>());

        Frame = allImages.Find(image => image.name == "Frame");
        Image = allImages.Find(image => image.name == "Image");
        Name = GetComponentInChildren<AbilityNameLabel>().GetComponent<Text>();
        Shade = GetComponentInChildren<AbilityShade>().GetComponent<RectTransform>();
    }
}
