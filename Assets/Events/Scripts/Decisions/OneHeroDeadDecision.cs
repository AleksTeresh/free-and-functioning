using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/OneHeroDead")]
    public class OneHeroDeadDecision : Decision
    {
        public Unit[] heroesToStayAlive;

        public override bool Decide(StateController controller)
        {
            var aliveHeroes = controller.player.GetUnits();

            foreach (var shouldBeAliveHero in heroesToStayAlive)
            {
                if (!aliveHeroes.Exists(unit => unit.name == shouldBeAliveHero.name || unit.name == shouldBeAliveHero.name + "(Clone)"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
