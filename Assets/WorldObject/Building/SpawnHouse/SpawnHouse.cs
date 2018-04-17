using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : Building {
    [Header("Spawning")]
    public float spawnRateUpperLimit = 100.0f;
    public float spawnRateLowerLimit = 0.0f;

    public float meleeSwarmlingSpawnRate = 0.3f;
    public float meleeSwarmlingSpawnAccel = 0.0f;

    public float rangeSwarmlingSpawnRate = 0.2f;
    public float rangeSwarmlingSpawnAccel = 0.0f;

    public float assassinSpawnRate = 0.1f;
    public float assassinSpawnAccel = 0.0f;

    public float hulkSpawnRate = 0.1f;
    public float hulkSpawnAccel = 0.0f;

    public float damageDealerSpawnRate = 0.1f;
    public float damageDealerSpawnAccel = 0.0f;

    public float debufferSpawnRate = 0.1f;
    public float debufferSpawnAccel = 0.0f;

    public float crowdControlSpawnRate = 0.1f;
    public float crowdControlSpawnAccel = 0.0f;

    public MeleeSwarmling meleeSwarmling;
    public RangeSwarmling rangeSwarmling;
    public Assassin assassin;
    public Hulk hulk;
    public DamageDealer damageDealer;
    public DebufferEnemy debuffer;
    public CrowdControlEnemy crowdControl;

    public float spawnInterval = 5.0f;
    public float spawnIntervalAccel = 0.0f;
    public float spawnIntervalLowerLimit = 0.0f;
    public float spawnIntervalUpperLimit = 60.0f;

    protected override void Start()
    {
        base.Start();

        actions = new string[] {
            meleeSwarmling.name,
            rangeSwarmling.name,
            assassin.name,
            hulk.name,
            damageDealer.name,
            debuffer.name,
            crowdControl.name
        };
    }

    protected override void Update()
    {
        base.Update();

        // adjust spawn interval
        spawnInterval = Mathf.Min(
            spawnIntervalUpperLimit,
            Mathf.Max(spawnIntervalLowerLimit, spawnInterval + spawnIntervalAccel * Time.deltaTime)
        );

        // adjust spawn rate of unit types
        meleeSwarmlingSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, meleeSwarmlingSpawnRate + meleeSwarmlingSpawnAccel * Time.deltaTime)
        );
        rangeSwarmlingSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, rangeSwarmlingSpawnRate + rangeSwarmlingSpawnAccel * Time.deltaTime)
        );
        assassinSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, assassinSpawnRate + assassinSpawnAccel * Time.deltaTime)
        );
        hulkSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, hulkSpawnRate + hulkSpawnAccel * Time.deltaTime)
        );
        damageDealerSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, damageDealerSpawnRate + damageDealerSpawnAccel * Time.deltaTime)
        );
        debufferSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, debufferSpawnRate + debufferSpawnAccel * Time.deltaTime)
        );
        crowdControlSpawnRate = Mathf.Min(
            spawnRateUpperLimit,
            Mathf.Max(spawnRateLowerLimit, crowdControlSpawnRate + crowdControlSpawnAccel * Time.deltaTime)
        );
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
        return new float[]
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
}
