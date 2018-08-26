using RTS;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class PostLevelSelectMenu : MonoBehaviour
    {
        private StartLevelButton newGameButton;
        private LoadButton continueButton;

        public void Start()
        {
            newGameButton = GetComponentInChildren<StartLevelButton>();
            continueButton = GetComponentInChildren<LoadButton>();
        }

        public void SetScene(string sceneName)
        {
            if (!newGameButton || !continueButton)
            {
                newGameButton = GetComponentInChildren<StartLevelButton>();
                continueButton = GetComponentInChildren<LoadButton>();
            }

            newGameButton.relatedSceneName = sceneName;
            continueButton.relatedSceneName = sceneName;

            var newGameUIButton = newGameButton.GetComponent<Button>();
            newGameUIButton.Select();
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);

            var levelSelectMenu = Instantiate(ResourceManager.GetUIElement("LevelSelectMenu"));

            var canvas = transform.parent.GetComponentInChildren<Canvas>();
            if (canvas && levelSelectMenu)
            {
                levelSelectMenu.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>(), false);
            }
        }
    }
}
