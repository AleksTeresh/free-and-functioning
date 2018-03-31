using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionIndicator : MonoBehaviour {

    public Text NameField { get; private set; }
    public Slider HealthSlider { get; private set; }
    public Image Avatar { get; private set; }

    // Use this for initialization
    void Start () {
        NameField = GetComponentInChildren<Text>();
        HealthSlider = GetComponentInChildren<Slider>();
        Avatar = GetComponentInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
