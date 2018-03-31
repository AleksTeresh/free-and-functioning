using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

    protected Slider healthSlider;
    protected Text nameLabel;
    protected Image avatar;
    // TODO: add effects wrapper and effects list itself

    protected Unit unit;

    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
        nameLabel = GetComponentInChildren<Text>();
        avatar = GetComponentInChildren<Image>();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (unit)
        {
            healthSlider.maxValue = unit.maxHitPoints;
            healthSlider.value = unit.hitPoints;

            nameLabel.text = unit.name;

            // TODO: uncomment this line once we have actual avatar
            // avatar.sprite = unit.avatar;
        }
    }
}
