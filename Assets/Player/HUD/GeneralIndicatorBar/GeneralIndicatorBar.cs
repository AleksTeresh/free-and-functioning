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
            attackModeIndicator.SetSprite(isMulti ? multiAttackIcon : singleAttackIcon);
        }
    }

    public void SetFormationMode(FormationType formationType)
    {
        if (formationModeIndicator)
        {
            formationModeIndicator.SetSprite(formationType == FormationType.Manual ? manualFormationIcon : autoFormationIcon);
        }
    }
}
