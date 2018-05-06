using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Decisions/MajorObjectsAreDeadDecision")]
    public class MajorObjectsAreDeadDecision : Decision
    {
        public WorldObject[] prefabsToStayAlive;

        public override bool Decide(StateController controller)
        {
            List<WorldObject> toStayAlives = new List<WorldObject>();

            foreach (var prefab in prefabsToStayAlive)
            {
                var toStayAliveObj = new List<Object>(FindObjectsOfType(prefab.GetType()))
                    .FindLast(obj => obj.name == prefab.name);

                if (toStayAliveObj)
                {
                    var toStayAlive = (WorldObject)toStayAliveObj;

                    if (toStayAlive && toStayAlive.hitPoints > 0)
                    {
                        toStayAlives.Add(toStayAlive);
                    }
                }
            }

            return toStayAlives.Count == 0;
        }
    }
}
