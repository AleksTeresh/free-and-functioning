using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    [HideInInspector] public string relatedSceneName;

    private Button button;

    public void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => OnSelect());
    }

    public void OnSelect ()
    {
        SceneManager.LoadScene(relatedSceneName);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
