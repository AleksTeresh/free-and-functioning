using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/AllHeroesDead")]
    public class AllHeroesDeadDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.player.GetUnits().Count == 0;
        }
    }
}
