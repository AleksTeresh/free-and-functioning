using UnityEngine;
using RTS;

namespace AI.Spawner
{
    [CreateAssetMenu(menuName = "AI/Actions/BuildingAI/Spawner/SpawnRandom")]
    public class SpawnRandomAction : BuildingAction
    {
        protected override void DoAction(BuildingStateController controller)
        {
            Building building = controller.building;

            if (building is SpawnHouse)
            {
                var spawnHouse = (SpawnHouse) building;

                if (controller.spawnTimer > spawnHouse.spawnInterval)
                {
                    HandleSpawning(spawnHouse);

                    controller.spawnTimer = 0;
                }
            }
        }

        private void HandleSpawning(SpawnHouse building)
        {
            string nameOfNextSpawn = building.GetActions()[RandomUtils.Choose(building.GetProbabilityArray())];

            if (nameOfNextSpawn != null && nameOfNextSpawn != "")
            {
                // TODO: change hardcoded center shift to the one derived from a variable
                var newSpawnPoint = WorkManager.GetRandomDestinationPoint(building.transform.position + Vector3.back * 30, 30);
                building.SetSpawnPoint(newSpawnPoint);
                building.PerformAction(nameOfNextSpawn);
            }
        }
    }
}