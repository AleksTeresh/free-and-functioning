using Menu;
using RTS;
using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
    public string sceneName;

    private Canvas canvas;

    public void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnSelect()
    {
        var postSelectMenuObj = GameObject.Instantiate(ResourceManager.GetUIElement("PostLevelSelectMenu"));
        var postSelectMenu = postSelectMenuObj.GetComponent<PostLevelSelectMenu>();
        var rectTransform = postSelectMenuObj.GetComponent<RectTransform>();

        rectTransform.SetParent(canvas.GetComponent<RectTransform>(), false);

        postSelectMenu.SetScene(sceneName);

        var levelSelectMenu = transform.root.GetComponentInChildren<LevelSelectMenu>();
        if (levelSelectMenu)
        {
            levelSelectMenu.DestroySelf();
        }
    }
}
