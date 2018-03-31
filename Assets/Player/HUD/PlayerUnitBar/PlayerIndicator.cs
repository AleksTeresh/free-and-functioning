using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour {

    private Slider healthSlider;
	// Use this for initialization
	void Start () {
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.value = 100;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
