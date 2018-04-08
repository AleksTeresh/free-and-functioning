using UnityEngine;
using RTS;
using UnityEngine.AI;

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
            var destinationPoint = self.destination + offset.normalized * WALK_RADIUS;
            // var destinationPoint = WorkManager.GetClosestPointOnNavMesh(initialDestination, "Walkable");
            float distance = Vector3.Distance(destinationPoint, self.transform.position);
            bool reachable = NavMesh.CalculatePath(
                self.transform.position,
                destinationPoint,
                WorkManager.GetNavMeshAreaFromName("Walkable"),
                new NavMeshPath()
            );
            if (Vector3.Distance(self.transform.position, destinationPoint) < 1 || !reachable)
            {
                destinationPoint = WorkManager.GetPerpendicularDestinationPoint(self, destinationPoint, WALK_RADIUS);
            }

            controller.unit.StartMove(destinationPoint);
        }
    }
}