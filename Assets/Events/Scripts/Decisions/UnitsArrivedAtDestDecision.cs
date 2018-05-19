using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/UnitsArrivedAtDest")]
    public class UnitsArrivedAtDestDecision : Decision
    {
        public Flag destinationPrefab;
        public Unit[] unitPrefabs;
        public float radius = 7;

        // private bool triggerred = false;

        private void OnEnable()
        {
            var destination = new List<Flag>(FindObjectsOfType<Flag>()).Find(p => p.name == destinationPrefab.name);

            if (destination)
                destination.triggerred = false;
        }

        public override bool Decide(StateController controller)
        {
            var destination = new List<Flag>(FindObjectsOfType<Flag>()).Find(p => p.name == destinationPrefab.name);

            if (!destination) return false;

            if (destination.triggerred) return false;

            var destPos = destination.transform.position;
            var controllerUnits = controller.player.GetUnits();

            foreach (var unitPrefab in unitPrefabs)
            {
                var unit = controllerUnits.Find(p => p.name == unitPrefab.name);

                if (!unit) return false;

                var unitPos = unit.transform.position;

                if ((unitPos - destPos).sqrMagnitude > radius * radius)
                {
                    return false;
                }
            }

            destination.triggerred = true;
            return true;
        }
    }
}
