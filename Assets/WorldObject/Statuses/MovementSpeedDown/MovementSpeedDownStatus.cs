using UnityEngine.AI;

namespace Statuses
{
    public class MovementSpeedDownStatus : Status
    {
        public float movementSpeedDebuff = 0.25f;
        private NavMeshAgent navMeshAgent;
        private float absoluteSpeedDebuff;
        // private float originalSpeed;

        protected override void OnStatusStart()
        {
            if (target)
            {
                navMeshAgent = target.GetComponent<NavMeshAgent>();
                // originalSpeed = navMeshAgent.speed;

                if (navMeshAgent)
                {
                    absoluteSpeedDebuff = navMeshAgent.speed * movementSpeedDebuff;

                    navMeshAgent.speed -= absoluteSpeedDebuff;
                }
            }
        }

        protected override void OnStatusEnd()
        {
            if (navMeshAgent)
            {
                navMeshAgent.speed += absoluteSpeedDebuff;
            }
        }
    }
}
