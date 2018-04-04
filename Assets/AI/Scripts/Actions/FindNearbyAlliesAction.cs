using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTS;

[CreateAssetMenu(menuName = "AI/Actions/FindNearbyAllies")]
public class FindNearbyAlliesAction : Action
{

    public override void Act(StateController controller)
    {
        FindNearbyAllies(controller);
    }

    private void FindNearbyAllies(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, unit.detectionRange);

        List<WorldObject> allyObjects = nearbyObjects
            .Where(p => p.GetPlayer() != null && p.GetPlayer() == unit.GetPlayer())
            .ToList();

        controller.nearbyAllies = allyObjects;
    }
}
