using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/SpawnEnemiesIfNoEnemies")]
    public class SpawnEnemiesIfNoEnemiesAction : SpawnEnemiesAction
    {

        public override void Act(StateController controller)
        {
            if (!UnitManager.EnemyUnitsExist(controller.player))
            {
                base.Act(controller);
            }
        }
    }
}
