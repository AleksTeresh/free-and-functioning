using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/UnitsInDefenceMode")]
    public class UnitsInDefenceModeDecision : Decision
    {
        public Unit[] unitPrefabs;

        public override bool Decide(StateController controller)
        {
            var controllerUnits = controller.player.GetUnits();

            foreach (var unitPrefab in unitPrefabs)
            {
                var unit = controllerUnits.Find(p => p.name == unitPrefab.name);

                if (!unit || !unit.holdingPosition)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
