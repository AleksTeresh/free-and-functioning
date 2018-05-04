using UnityEngine;
using RTS;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/NoVisibleEnemyUnits")]
    public class NoVisibleEnemyUnitsDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return UnitManager.GetPlayerVisibleEnemies(controller.player).Count == 0;
        }
    }
}
