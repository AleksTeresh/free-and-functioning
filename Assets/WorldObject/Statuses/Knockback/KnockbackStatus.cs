using System;
using UnityEngine;
using UnityEngine.AI;

namespace Statuses
{
    public class KnockbackStatus : Status
    {
        public float distance = 10.0f;
        public float speed = 5.0f;

        private NavMeshAgent navMeshAgent;
        private float originalSpeed;
        private Vector3 knockbackDirection;

        protected override void OnStatusStart()
        {
            maxDuration = distance / speed;

            if (target && (projectileInflicter || (aoeInflicter && inflicter)))
            {
                // Copy projectile's direction, because projectile will be destroyed further in this frame
                knockbackDirection = CalculateKnockbackDirection();

                navMeshAgent = target.GetComponent<NavMeshAgent>();

                if (navMeshAgent)
                {
                    originalSpeed = navMeshAgent.speed;
                    navMeshAgent.speed = 0;
                }
            }
        }

        protected override void AffectTarget()
        {
            if (target)
            {
                // Move target in the initial projectile direction
                float positionChange = Time.deltaTime * speed;
                target.transform.position += (positionChange * knockbackDirection);
            }
        }

        protected override void OnStatusEnd()
        {
            if (navMeshAgent)
            {
                navMeshAgent.speed = originalSpeed;
            }
        }

        private Vector3 CalculateKnockbackDirection()
        {
            if (projectileInflicter)
            {
                return new Vector3(projectileInflicter.transform.forward.x, 0.0f, projectileInflicter.transform.forward.z);
            }
            else if (aoeInflicter && inflicter)
            {
                Vector3 direction;

                // Check if AoE and target have different positions
                // Sometimes Y coordinates of AoE and target don't match exactly, thus perform check only on X and Z
                if (target.transform.position.x == aoeInflicter.transform.position.x && target.transform.position.z == aoeInflicter.transform.position.z)
                {
                    direction = (target.transform.position - inflicter.transform.position);
                }
                // If AoE and target positions are the same, push target away from the inflicter
                else
                {
                     direction = (target.transform.position - aoeInflicter.transform.position);
                }

                direction.y = 0.0f;
                direction.Normalize();

                return direction;
            }

            throw new Exception("Either inflicter and aoeInflicter or projectileInflicter must be set");
        }
    }
}
