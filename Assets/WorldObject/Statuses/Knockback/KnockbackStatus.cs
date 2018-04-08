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

            if (target && projectileInflicter)
            {
                // Copy projectile's direction, because projectile will be destroyed further in this frame
                knockbackDirection = new Vector3(projectileInflicter.transform.forward.x, 0.0f, projectileInflicter.transform.forward.z);

                navMeshAgent = target.GetComponent<NavMeshAgent>();
                originalSpeed = navMeshAgent.speed;
                navMeshAgent.speed = 0;
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
    }
}
