using UnityEngine;
using UnityEngine.SceneManagement;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Action/LoadScene")]
    public class LoadSceneAction : Action
    {
        public string sceneName;

        public override void Act(StateController controller)
        {
            if (sceneName != null && sceneName != "")
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
