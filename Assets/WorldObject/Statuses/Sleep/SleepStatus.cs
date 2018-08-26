using RTS;
using RTS.Constants;

namespace Statuses
{
    public class SleepStatus : AiStateStatus
    {
        protected override void AffectTarget()
        {
            if (target && target.GetStateController())
            {
                var targetStateController = target.GetStateController();

                targetStateController.TransitionToState(ResourceManager.GetAiState(AIStates.STUNNED));

                if (target.IsUnderAttack())
                {
                    FinishStatus();
                }
            }
        }
    }
}

