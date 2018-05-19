using UnityEngine;
using UnityEngine.UI;
using RTS;
using Persistence;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour {
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.enabled = LoadManager.SaveExists(Constants.LAST_SAVE_FILENAME);
    }

    public void ContinueGame ()
    {
        if (LoadManager.SaveExists(Constants.LAST_SAVE_FILENAME))
        {
            string sceneName = LoadManager.GetSavedSceneName(Constants.LAST_SAVE_FILENAME);

            if (sceneName != null && sceneName != "")
            {
                ResourceManager.LevelName = sceneName;
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
