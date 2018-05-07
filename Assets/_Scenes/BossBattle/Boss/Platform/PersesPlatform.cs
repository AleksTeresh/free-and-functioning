using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersesPlatform : Unit {
    private SpawnHouse head;
    private BossPart[] bodyParts;

    protected override void AwakeObj()
    {
        base.AwakeObj();

        var bossWrapper = GetComponentInParent<Boss>();
        head = bossWrapper.GetComponentInChildren<SpawnHouse>();
        bodyParts = bossWrapper.GetComponentsInChildren<BossPart>();
    }

    protected override void Update()
    {
        base.Update();

        var ownPosition = transform.position;
        var ownRotation = transform.rotation;

        if (head)
        {
            head.transform.position = new Vector3(ownPosition.x, head.transform.position.y, ownPosition.z);
            head.transform.rotation = ownRotation;

            head.CalculateBounds();
        }

        if (bodyParts != null)
        {
            foreach (var bodyPart in bodyParts)
            {
                if (!bodyPart) continue;

                bodyPart.transform.position = new Vector3(ownPosition.x, bodyPart.transform.position.y, ownPosition.z);

                if (!bodyPart.aiming && !bodyPart.GetStateController().chaseTarget)
                {
                    bodyPart.SetAimRotation(ownRotation);
                }

                bodyPart.CalculateBounds();
            }
        }
    }

    void OnDestroy()
    {
        var ownPosition = transform.position;
        /*
                if (head)
                {
                    head.transform.position = new Vector3(
                        head.transform.position.x,
                        head.transform.position.y - ownPosition.y,
                        head.transform.position.z
                    );
                }  */
        var bodyPartList = new List<BossPart>(bodyParts);

        if (bodyParts != null && bodyPartList.Exists(p => p is PersesBody))
        {
            var body = bodyPartList.Find(p => p is PersesBody);

            if (body)
            {
                float initialBodyY = body.transform.position.y;

                var bodyNavMesh = body.gameObject.AddComponent<NavMeshAgent>() as NavMeshAgent;

                if (bodyNavMesh)
                {
                    bodyNavMesh.speed = 0;
                    bodyNavMesh.baseOffset = initialBodyY;
                }

                body.CalculateBounds();
            }       
        }
    }

    public override bool IsMajor()
    {
        return true;
    }
}
