using System.Collections.Generic;
using UnityEngine;
using Formation;

public class UpperBar : MonoBehaviour {

    private TextIndicator attackModeIndicator;
    private TextIndicator formationModeIndicator;
    private TextIndicator enemyCountIndicator;

    // Use this for initialization
    void Start () {
        var textIndicators = new List<TextIndicator>(GetComponentsInChildren<TextIndicator>());

        attackModeIndicator = textIndicators.Find(ind => ind.name.Contains("AttackMode"));
        formationModeIndicator = textIndicators.Find(ind => ind.name.Contains("FormationMode"));
        enemyCountIndicator = textIndicators.Find(ind => ind.name.Contains("EnemyCount"));
    }
	
	public void SetAttackMode (bool isMulti)
    {
        if (attackModeIndicator)
        {
            attackModeIndicator.SetColor(isMulti ? Color.red : Color.green);
            attackModeIndicator.SetText(isMulti ? "Multi Attack" : "Single Attack");
        }
    }

    public void SetFormationMode(FormationType formationType)
    {
        if (formationModeIndicator)
        {
            // uncomment the line below if needed
            // formationModeIndicator.SetColor(isMulti ? Color.red : Color.green);
            formationModeIndicator.SetText(formationType == FormationType.Manual ? "Manual Formation" : "Auto Formation");
        }
    }

    public void SetEnemyCount(int count)
    {
        if (enemyCountIndicator)
        {
            // uncomment the line below if needed
            // enemyCountIndicator.SetColor(isMulti ? Color.red : Color.green);
            enemyCountIndicator.SetText("Enemy Count: " + count);
        }
    }
}
