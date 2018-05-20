using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/AddExistingUnitToPlayer")]
    public class AddExistingUnitToPlayerAction : Action
    {
        public Unit unitPrefab;
        public int siblingIndex = -1;

        public override void Act(StateController controller)
        {
            var allUnits = FindObjectsOfType<Unit>();

            var requiredUnit = allUnits.First(unit => unit.name == unitPrefab.name);

            if (requiredUnit) {
                controller.player.AddUnit(
                    unitPrefab.name,
                    requiredUnit.transform.position,
                    requiredUnit.transform.position,
                    requiredUnit.transform.rotation,
                    null,
                    siblingIndex
                );

                Destroy(requiredUnit.gameObject);
            }
        }
    }
}
