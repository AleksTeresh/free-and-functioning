using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/RunAway")]
public class RunAwayAction : Action
{
    private static readonly float WALK_RADIUS = 10;

    public override void Act(StateController controller)
    {
        RunAway(controller);
    }

    private void RunAway(StateController controller)
    {
        var self = controller.navMeshAgent;
        var target = controller.chaseTarget;

        if (target)
        {
            var offset = self.transform.position - target.transform.position;
            // run away from the target (oppiosite direction of where the target is relative to the self)
            controller.unit.StartMove(self.destination + offset.normalized * WALK_RADIUS);

            WorkManager.GetRandomDestinationPoint(self, WALK_RADIUS);
        }
    }
}