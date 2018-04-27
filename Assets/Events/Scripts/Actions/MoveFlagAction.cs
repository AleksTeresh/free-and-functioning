using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/MoveFlag")]
    public class MoveFlagAction : Action
    {
        public float deltaX;
        public float deltaY;
        public float deltaZ;

        public override void Act(StateController controller)
        {
            var flag = FindObjectOfType<Flag>();

            if (flag)
            {
                var oldPosition = flag.transform.position;
                flag.transform.position = new Vector3(oldPosition.x + deltaX, oldPosition.y + deltaY, oldPosition.z + deltaZ);
            }
        }
    }
}
