using System.Collections.Generic;
using UnityEngine;

public class UnitBar : MonoBehaviour {
    private Indicators indicatorsWrapper;
    private List<Indicator> indicators;

    public Indicators GetIndicatorsWrapper()
    {
        return indicatorsWrapper;
    }

    public List<Indicator> GetIndicators()
    {
        return indicators;
    }

    // Use this for initialization
    void Start()
    {
        indicatorsWrapper = GetComponentInChildren<Indicators>();
        indicators = new List<Indicator>(indicatorsWrapper.GetComponentsInChildren<Indicator>());
    }

    // Update is called once per frame
    public void Update()
    {
        indicators = new List<Indicator>(indicatorsWrapper.GetComponentsInChildren<Indicator>());
    }
}
