using System.Collections.Generic;
using UnityEngine;
using Formation;

public class EnemyCountBar : MonoBehaviour {

	private TextIndicator enemyCountIndicator;

	// Use this for initialization
	void Start () {
		var textIndicators = new List<TextIndicator>(GetComponentsInChildren<TextIndicator>());

		enemyCountIndicator = textIndicators.Find(ind => ind.name.Contains("EnemyCount"));
	}
	
	public void SetEnemyCount(int count)
	{
		if (enemyCountIndicator)
		{
			// uncomment the line below if needed
			// enemyCountIndicator.SetColor(isMulti ? Color.red : Color.green);
			enemyCountIndicator.SetText("Enemies: " + count);
		}
	}
}
