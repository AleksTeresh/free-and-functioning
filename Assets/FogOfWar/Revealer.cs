using UnityEngine.AI;

namespace Assets.FogOfWar
{
    public class Revealer
    {
        public Revealer(WorldObject worldObject, NavMeshAgent navMeshAgent)
        {
            WorldObject = worldObject;
            NavMeshAgent = navMeshAgent;
        }

        public WorldObject WorldObject { get; set; }
        public NavMeshAgent NavMeshAgent { get; set; }
    }
}
