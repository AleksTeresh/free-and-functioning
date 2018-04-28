using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/UnitsHaveEnoughHealth")]
    public class UnitsHaveEnoughHealthDecision : Decision
    {
        public Unit[] unitPrefabs;
        public float requiredHealthPercentage = 1f;

        public override bool Decide(StateController controller)
        {
            var controllerUnits = controller.player.GetUnits();

            foreach (var unitPrefab in unitPrefabs)
            {
                var unit = controllerUnits.Find(p => p.name == unitPrefab.name);

                if (!unit || ((float)unit.hitPoints / (float)unit.maxHitPoints) < requiredHealthPercentage)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
