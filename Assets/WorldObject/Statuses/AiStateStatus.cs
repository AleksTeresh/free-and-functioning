using RTS;

namespace Statuses
{
    public class AiStateStatus : Status
    {
        protected string previousState;

        protected override void OnStatusStart()
        {
            // the method is to be overriden
        }

        protected override void OnStatusEnd()
        {
            if (target && target.GetStateController())
            {
                var controller = target.GetStateController();
                controller.TransitionToState(ResourceManager.GetAiState(controller.GetDefaultState().name));
            }
        }
    }

}
