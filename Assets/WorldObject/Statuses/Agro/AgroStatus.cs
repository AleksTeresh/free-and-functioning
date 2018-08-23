using RTS;
using RTS.Constants;

namespace Statuses
{
    public class AgroStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                if (targetStateController)
                {
                    targetStateController.chaseTarget = inflictor;
                    targetStateController.TransitionToState(ResourceManager.GetAiState(AIStates.UNDER_AGRO));
                }
            }
        }
    }
}

