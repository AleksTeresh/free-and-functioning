using UnityEngine;
using UnityEngine.UI;
using RTS.Constants;
using Persistence;

public class LoadButton : MonoBehaviour {
    private Button button;

    [HideInInspector] public string relatedSceneName = "";

    private LevelLoader leveLoader;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => ContinueGame());

        leveLoader = FindObjectOfType<LevelLoader>();
    }

    private void Update()
    {
        button.interactable = relatedSceneName != null &&
            relatedSceneName != "" &&
            LoadManager.SaveExists(relatedSceneName + PersistanceConstants.SAVE_FILENAME_POSTFIX);
    }

    public void ContinueGame ()
    {
        if (relatedSceneName != null && relatedSceneName != "" && LoadManager.SaveExists(relatedSceneName + PersistanceConstants.SAVE_FILENAME_POSTFIX))
        {
            string sceneName = relatedSceneName;

            if (sceneName != null && sceneName != "")
            {
                Time.timeScale = 1.0f;
                leveLoader.LoadSavedScene(sceneName);
            }
        }
    }

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
