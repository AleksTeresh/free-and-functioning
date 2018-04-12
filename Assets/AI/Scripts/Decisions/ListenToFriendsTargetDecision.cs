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

        if (unit.CanAttack())
        {
            // if there is no common target, look for nearby enemies
            foreach (WorldObject nearbyObject in controller.nearbyAllies)
            {
                if (
                    nearbyObject &&
                    unit.ObjectId != nearbyObject.ObjectId &&
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