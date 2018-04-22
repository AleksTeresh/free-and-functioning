using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class PersesBody : BossPart
{
    private SpawnHouse head;
    private BossPart[] bodyParts;

    protected override void Awake()
    {
        base.Awake();

        var bossWrapper = GetComponentInParent<Boss>();
        head = bossWrapper.GetComponentInChildren<SpawnHouse>();
        bodyParts = bossWrapper.GetComponentsInChildren<BossPart>();
    }

    protected override void Update()
    {
        base.Update();

        var navMesh = GetComponent<NavMeshAgent>();

        if (navMesh)
        {
            var ownPosition = transform.position;
            var ownRotation = transform.rotation;

            if (head)
            {
                head.transform.position = new Vector3(ownPosition.x, head.transform.position.y, ownPosition.z);
                // head.transform.rotation = ownRotation;
                head.CalculateBounds();
            }

            if (bodyParts != null)
            {
                foreach (var bodyPart in bodyParts)
                {
                    if (!bodyPart) continue;

                    bodyPart.transform.position = new Vector3(ownPosition.x, bodyPart.transform.position.y, ownPosition.z);
                    // bodyPart.transform.rotation = ownRotation;

                    bodyPart.CalculateBounds();
                }
            }
        }
    }

    void OnDestroy()
    {
        var ownPosition = transform.position;

        if (bodyParts != null)
        {
            // var body = bodyPartList.Find(p => p is PersesBody);
            // float initialBodyY = body.transform.position.y;

            for (int i = 0; i < bodyParts.Length; i++)
            {
                var bodyPart = bodyParts[i];

                if (bodyPart is PersesBody) continue;

                bodyPart.transform.position = new Vector3(
                    bodyPart.transform.position.x + 10 * i,
                    bodyPart.GetSelectionBounds().extents.y / 2,
                    bodyPart.transform.position.z
                );

                var bodyNavMesh = bodyPart.gameObject.AddComponent<NavMeshAgent>() as NavMeshAgent;
                bodyNavMesh.speed = 5;
                bodyNavMesh.baseOffset = bodyNavMesh.height / 2;

                bodyPart.CalculateBounds();
            }
        }

        if (head)
        {
            

            var headNavMesh = head.gameObject.AddComponent<NavMeshAgent>() as NavMeshAgent;
            headNavMesh.speed = 0;
            headNavMesh.baseOffset = -head.GetSelectionBounds().extents.y;

            head.transform.position = new Vector3(
                head.transform.position.x - 10,
                0,
                head.transform.position.z
            );

            head.CalculateBounds();
        }
    }

    public override bool IsMajor()
    {
        return true;
    }
}