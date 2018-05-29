using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitPanel : MonoBehaviour {

    private Indicator unitIndicator;
    private AbilityBar abilityBar;

	// Use this for initialization
	public void Start () {
        abilityBar = GetComponentInChildren<AbilityBar>();
        unitIndicator = GetComponentInChildren<Indicator>();
    }
	
	public AbilityBar GetAblityBar()
    {
        return abilityBar;
    }

    public Indicator GetIndicator ()
    {
        return unitIndicator;
    }
}
