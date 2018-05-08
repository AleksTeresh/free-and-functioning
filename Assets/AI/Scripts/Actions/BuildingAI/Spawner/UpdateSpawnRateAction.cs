using UnityEngine;
using RTS;

namespace AI.Spawner
{
    [CreateAssetMenu(menuName = "AI/Actions/BuildingAI/Spawner/UpdateSpawnRate")]
    public class UpdateSpawnRateAction : BuildingAction
    {
        protected override void DoAction(BuildingStateController controller)
        {
            Building building = controller.building;

            if (building is SpawnHouse)
            {
                var spawnHouse = (SpawnHouse)building;

                UpdateSpawnRates(spawnHouse);
            }
        }

        private void UpdateSpawnRates(SpawnHouse spawnHouse)
        {
            // adjust spawn interval
            spawnHouse.spawnInterval = Mathf.Min(
                spawnHouse.spawnIntervalUpperLimit,
                Mathf.Max(spawnHouse.spawnIntervalLowerLimit, spawnHouse.spawnInterval + spawnHouse.spawnIntervalAccel * Time.deltaTime)
            );

            // adjust spawn rate of indicatedObject types
            spawnHouse.meleeSwarmlingSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.meleeSwarmlingSpawnRate + spawnHouse.meleeSwarmlingSpawnAccel * Time.deltaTime)
            );
            spawnHouse.rangeSwarmlingSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.rangeSwarmlingSpawnRate + spawnHouse.rangeSwarmlingSpawnAccel * Time.deltaTime)
            );
            spawnHouse.assassinSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.assassinSpawnRate + spawnHouse.assassinSpawnAccel * Time.deltaTime)
            );
            spawnHouse.hulkSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.hulkSpawnRate + spawnHouse.hulkSpawnAccel * Time.deltaTime)
            );
            spawnHouse.damageDealerSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.damageDealerSpawnRate + spawnHouse.damageDealerSpawnAccel * Time.deltaTime)
            );
            spawnHouse.debufferSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.debufferSpawnRate + spawnHouse.debufferSpawnAccel * Time.deltaTime)
            );
            spawnHouse.crowdControlSpawnRate = Mathf.Min(
                spawnHouse.spawnRateUpperLimit,
                Mathf.Max(spawnHouse.spawnRateLowerLimit, spawnHouse.crowdControlSpawnRate + spawnHouse.crowdControlSpawnAccel * Time.deltaTime)
            );
        }
    }
}
