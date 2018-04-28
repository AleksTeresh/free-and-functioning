using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/DestroyAllEnemyUnits")]
    public class DestroyAllEnemyUnitsAction : Action
    {
        public override void Act(StateController controller)
        {
            var enemyPlayers = new List<Player>(FindObjectsOfType<Player>())
                .Where(p => p.username != controller.player.username)
                .ToList();

            enemyPlayers.ForEach(p => p.GetUnits().ForEach(unit => Destroy(unit.gameObject)));
        }
    }
}
