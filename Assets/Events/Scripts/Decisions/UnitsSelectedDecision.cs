using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/UnitsSelected")]
    public class UnitsSelectedDecision : Decision
    {
        public Unit[] unitPrefabs;

        public override bool Decide(StateController controller)
        {
            var controllerUnits = controller.player.GetUnits();

            foreach (var unitPrefab in unitPrefabs)
            {
                var unit = controllerUnits.Find(p => p.name == unitPrefab.name);

                if (!unit || !controller.player.selectedObjects.Contains(unit))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
