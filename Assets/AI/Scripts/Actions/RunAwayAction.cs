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
        var unit = controller.unit;
        var target = controller.chaseTarget;

        if (target && (target.transform.position - self.transform.position).sqrMagnitude < unit.detectionRange * unit.detectionRange)
        {
            var offset = self.transform.position - target.transform.position;

            // run away from the target (oppiosite direction of where the target is relative to the self)
            var destinationPoint = self.destination + offset.normalized * WALK_RADIUS;
            // var destinationPoint = WorkManager.GetClosestPointOnNavMesh(initialDestination, "Walkable");
            float sqrDistance = (destinationPoint - self.transform.position).sqrMagnitude;
            bool reachable = NavMesh.CalculatePath(
                self.transform.position,
                destinationPoint,
                WorkManager.GetNavMeshAreaFromName("Walkable"),
                new NavMeshPath()
            );
            if (sqrDistance < 1 || !reachable)
            {
                destinationPoint = WorkManager.GetPerpendicularDestinationPoint(self, destinationPoint, WALK_RADIUS);
            }

            controller.unit.StartMove(destinationPoint);
        }
    }
}