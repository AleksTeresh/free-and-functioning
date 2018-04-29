using UnityEngine;
using RTS;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/NoEnemyUnits")]
    public class NoEnemyUnitsDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return !UnitManager.EnemyUnitsExist(controller.player);
        }
    }
}
