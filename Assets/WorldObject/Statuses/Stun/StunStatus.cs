using RTS;
using RTS.Constants;

namespace Statuses
{
    public class StunStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                targetStateController.TransitionToState(ResourceManager.GetAiState(AIStates.STUNNED));
            }
        }
    }
}
