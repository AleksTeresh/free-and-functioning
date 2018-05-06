using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/MajorObjectIsDeadDecision")]
    public class MajorObjectIsDeadDecision : Decision
    {
        public WorldObject prefabToStayAlive;

        public override bool Decide(StateController controller)
        {
            var toStayAlive = FindObjectOfType(prefabToStayAlive.GetType());

            if (toStayAlive is WorldObject)
            {
                var objectToStayAlive = (WorldObject)toStayAlive;

                if (objectToStayAlive && objectToStayAlive.hitPoints > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
