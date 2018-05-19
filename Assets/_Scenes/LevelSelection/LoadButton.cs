using UnityEngine;
using UnityEngine.UI;
using RTS;
using Persistence;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour {
    private Button button;

    [HideInInspector] public string relatedSceneName = "";

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => ContinueGame());
    }

    private void Update()
    {
        button.interactable = relatedSceneName != null &&
            relatedSceneName != "" &&
            LoadManager.SaveExists(relatedSceneName + Constants.SAVE_FILENAME_POSTFIX);
    }

    public void ContinueGame ()
    {
        if (relatedSceneName != null && relatedSceneName != "" && LoadManager.SaveExists(relatedSceneName + Constants.SAVE_FILENAME_POSTFIX))
        {
            string sceneName = LoadManager.GetSavedSceneName(relatedSceneName + Constants.SAVE_FILENAME_POSTFIX);

            if (sceneName != null && sceneName != "")
            {
                ResourceManager.LevelName = sceneName;
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
