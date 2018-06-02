using System.Collections.Generic;
using UnityEngine;
using Formation;

public class GeneralIndicatorBar : MonoBehaviour
{
    public Sprite multiAttackIcon;
    public Sprite singleAttackIcon;
    public Sprite autoFormationIcon;
    public Sprite manualFormationIcon;
    
    private TextIndicator attackModeIndicator;
    private TextIndicator formationModeIndicator;

    // Use this for initialization
    void Start () {
        var textIndicators = new List<TextIndicator>(GetComponentsInChildren<TextIndicator>());

        attackModeIndicator = textIndicators.Find(ind => ind.name.Contains("AttackMode"));
        formationModeIndicator = textIndicators.Find(ind => ind.name.Contains("FormationMode"));
    }
	
    public void SetAttackMode (bool isMulti)
    {
        if (attackModeIndicator)
        {
//            attackModeIndicator.SetColor(isMulti ? Color.red : Color.green);
//            attackModeIndicator.SetText(isMulti ? "Multi Attack" : "Single Attack");
            attackModeIndicator.SetSprite(isMulti ? multiAttackIcon : singleAttackIcon);
        }
    }

    public void SetFormationMode(FormationType formationType)
    {
        if (formationModeIndicator)
        {
            // uncomment the line below if needed
            // formationModeIndicator.SetColor(isMulti ? Color.red : Color.green);
//            formationModeIndicator.SetText(formationType == FormationType.Manual ? "Manual Formation" : "Auto Formation");
            formationModeIndicator.SetSprite(formationType == FormationType.Manual ? manualFormationIcon : autoFormationIcon);
        }
    }
}
