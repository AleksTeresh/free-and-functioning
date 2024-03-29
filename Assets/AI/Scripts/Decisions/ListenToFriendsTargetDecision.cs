﻿using UnityEngine;
/*
 * 
 * Should be used with computer controlled units
 */

namespace AI
{
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
            WorldObject unit = controller.controlledObject;
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

}
