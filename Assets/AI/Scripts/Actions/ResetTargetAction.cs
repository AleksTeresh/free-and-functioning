using UnityEngine;
using RTS;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/ResetTarget")]
public class ResetTargetAction : Action
{
    // private static readonly float WALK_RADIUS = 10;

    public override void Act(StateController controller)
    {
        RestTarget(controller);
    }

    private void RestTarget(StateController controller)
    {
        controller.chaseTarget = null;
    }
}