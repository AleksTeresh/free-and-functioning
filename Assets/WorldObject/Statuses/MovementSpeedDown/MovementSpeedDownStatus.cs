using UnityEngine.AI;

namespace Statuses
{
    public class MovementSpeedDownStatus : Status
    {
        public float movementSpeedDebuff = 0.25f;
        private NavMeshAgent navMeshAgent;
        private float originalSpeed;

        protected override void OnStatusStart()
        {
            if (target)
            {
                navMeshAgent = target.GetComponent<NavMeshAgent>();
                originalSpeed = navMeshAgent.speed;
                navMeshAgent.speed = navMeshAgent.speed * (1 - movementSpeedDebuff);
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
