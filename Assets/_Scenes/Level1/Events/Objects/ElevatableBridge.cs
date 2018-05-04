using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;
using UnityEngine.AI;

public class ElevatableBridge : MonoBehaviour {

    public float enemyCleanTriggerRadius = 200;

    private bool triggerred = false;
    private NavMeshSurface navMeshSurface;

    private void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        if (!triggerred)
        {
            var nearbyEnemyUnits = WorkManager.FindNearbyUnits(transform.position, enemyCleanTriggerRadius)
                .Where(p => !p.GetPlayer().human)
                .ToList();

            if (nearbyEnemyUnits.Count == 0)
            {
                triggerred = true;

                StartCoroutine(moveTheBridge());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyCleanTriggerRadius);
    }

    private IEnumerator<int> moveTheBridge()
    {
        for (int i = 0; i < 23; i++)
        {
            transform.position = new Vector3(
                transform.position.x + 1,
                transform.position.y,
                transform.position.z
            );

            yield return 0;
        }

    }
}
