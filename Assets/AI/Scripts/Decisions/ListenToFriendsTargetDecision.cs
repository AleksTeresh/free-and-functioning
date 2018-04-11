using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTS;
using UnityEngine;
/*
 * 
 * Should be used with computer controlled units
 */
[CreateAssetMenu(menuName = "AI/Decisions/ListenToFriendsTarget")]
public class ListenToFriendsTargetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetAvailable = ListenToFriendsTarget(controller);
        return targetAvailable;

    }

    private bool ListenToFriendsTarget(StateController controller)
    {
        Unit unit = controller.unit;
        Vector3 currentPosition = unit.transform.position;
        List<WorldObject> nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, unit.detectionRange);

        if (unit.CanAttack())
        {
            // if there is no common target, look for nearby enemies
            // List<WorldObject> friendlyObjects = new List<WorldObject>();
            foreach (WorldObject nearbyObject in nearbyObjects)
            {
                if (
                    unit.ObjectId != nearbyObject.ObjectId &&
                    nearbyObject && nearbyObject.GetPlayer() &&
                    nearbyObject.GetPlayer().username == unit.GetPlayer().username &&
                    nearbyObject.GetStateController() &&
                    nearbyObject.GetStateController().chaseTarget
                )
                {
                    controller.chaseTarget = nearbyObject.GetStateController().chaseTarget;

                    return true;
                }
            }
        }

        return false;
    }
}