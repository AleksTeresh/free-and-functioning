using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/SpawnEnemies")]
    public class SpawnEnemiesAction : Action
    {
        public Vector3 spawnPoint;
        public Vector3 rallyPoint;
        public Unit[] units;

        public override void Act(StateController controller)
        {
            var enemyPlayer = new List<Player>(FindObjectsOfType<Player>())
                .Find(p => p.username != controller.player.username);

            if (enemyPlayer)
            {
                for (int i = 0; i < units.Length; i++)
                {
                    if (!units[i]) return;

                    var exactSpawnPoint = spawnPoint + Vector3.right * 4 * i + Vector3.back * 4 * i;
                    var exactRallyPoint = rallyPoint + Vector3.right * 4 * i + Vector3.back * 4 * i;

                    enemyPlayer.AddUnit(
                        units[i].name,
                        exactSpawnPoint,
                        exactRallyPoint,
                        enemyPlayer.transform.rotation,
                        null
                    );
                }
            }
        }
    }
}
