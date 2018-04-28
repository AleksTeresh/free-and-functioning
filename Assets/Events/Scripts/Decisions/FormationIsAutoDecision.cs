using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/FormationIsAuto")]
    public class FormationIsAutoDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.formationManager &&
                controller.formationManager.CurrentFormationType == Formation.FormationType.Auto;
        }
    }
}
