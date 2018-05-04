using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using UnityEngine.AI;

public class SlidableBridge : MonoBehaviour {
    private bool triggerred = false;
    private NavMeshSurface navMeshSurface;

    public PressTrigger pressTrigger;

    private void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        if (!triggerred && pressTrigger && pressTrigger.IsPressed())
        {
            triggerred = true;
            StartCoroutine(moveTheBridge());
        }
    }

    private IEnumerator<int> moveTheBridge()
    {
        for (int i = 0; i < 123; i++)
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
