using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PersesBody : Unit
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

    void OnDestroy()
    {
        var ownPosition = transform.position;

        if (head)
        {
            head.transform.position = new Vector3(
                head.transform.position.x,
                head.transform.position.y - ownPosition.y,
                head.transform.position.z
            );
        }

        if (bodyParts != null)
        {
            foreach (var bodyPart in bodyParts)
            {
                bodyPart.transform.position = new Vector3(
                    bodyPart.transform.position.x,
                    bodyPart.transform.position.y - ownPosition.y,
                    bodyPart.transform.position.z
                );
            }
        }
    }

    public override bool IsMajor()
    {
        return true;
    }
}