using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/FormationIsCustom")]
    public class FormationIsCustomDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return controller.formationManager &&
                controller.formationManager.CurrentFormationType == Formation.FormationType.Manual;
        }
    }
}
