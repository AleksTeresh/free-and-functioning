using System.Linq;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/AgroWasUsedOnEnemies")]
    public class AgroWasUsedOnEnemiesDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            var units = controller.player.GetUnits();

            var tanker = units.Find(p => p is Tanker);
            bool agroWasUsedOnEnemies = tanker.GetStateController()
                .nearbyEnemies
                .Where(p => p.ActiveStatuses.Exists(status => status.statusName == "Agro")).Count() > 0;

            return agroWasUsedOnEnemies;
        }
    }
}
