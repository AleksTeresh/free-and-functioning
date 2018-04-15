﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : Building {
    public float meleeSwarmlingSpawnRate = 0.3f;
    public float rangeSwarmlingSpawnRate = 0.2f;
    public float assassinSpawnRate = 0.1f;
    public float hulkSpawnRate = 0.1f;
    public float damageDealerSpawnRate = 0.1f;
    public float debufferSpawnRate = 0.1f;
    public float crowdControlSpawnRate = 0.1f;

    public float spawnInterval = 5.0f;

    private float[] probabilityArray;

    protected override void Start()
    {
        base.Start();

        actions = new string[] {
            "MeleeSwarmling",
            "RangeSwarmling",
            "Assassin",
            "Hulk",
            "EnemyDamageDealer",
            "DebufferEnemy",
            "CrowdControlEnemy"
        };

        probabilityArray = new float[]
        {
            meleeSwarmlingSpawnRate,
            rangeSwarmlingSpawnRate,
            assassinSpawnRate,
            hulkSpawnRate,
            damageDealerSpawnRate,
            debufferSpawnRate,
            crowdControlSpawnRate
        };
    }

    public override void SetSpawnPoint(Vector3 spawnPoint)
    {
        base.SetSpawnPoint(spawnPoint);

        this.rallyPoint = spawnPoint;
    }

    public override void PerformAction(string actionToPerform)
    {
        base.PerformAction(actionToPerform);
        CreateUnit(actionToPerform);
    }

    public float[] GetProbabilityArray()
    {
        return probabilityArray;
    }
}
