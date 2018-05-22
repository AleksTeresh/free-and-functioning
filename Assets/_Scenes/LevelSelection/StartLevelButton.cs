using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    [HideInInspector] public string relatedSceneName;

    private Button button;
    private LevelLoader leveLoader;

    public void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => OnSelect());

        leveLoader = FindObjectOfType<LevelLoader>();
    }

    public void OnSelect ()
    {
        leveLoader.LoadNewScene(relatedSceneName);
        // SceneManager.LoadScene(relatedSceneName);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
