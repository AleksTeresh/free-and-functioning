using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RTS;
using RTS.Constants;
using Persistence;
using Events;
using UnityEngine.UI;
using System.Threading;

public class LevelLoader : MonoBehaviour
{
    private static int nextObjectId = 1;
    private static bool created = false;
    private bool initialised = false;

    private volatile GameData gameData = null;

    public Text[] loadTexts;

    private string sceneToLoad = "";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
            initialised = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (initialised)
        {
            if (SceneManager.GetActiveScene().name == "LevelLoading")
            {
                gameData = null;
                StartCoroutine(LoadNewSceneAsync(sceneToLoad));
            }
            else if (gameData != null && ResourceManager.LevelName != null && ResourceManager.LevelName != "")
            {
                LoadManager.LoadAssetsToScene(gameData);
                ResourceManager.LevelName = "";
            }
            else
            {
                SetObjectIds();
            }
            Time.timeScale = 1.0f;
            ResourceManager.MenuOpen = false;
        }
    }

    public int GetNewObjectId()
    {
        nextObjectId++;
        if (nextObjectId >= int.MaxValue) nextObjectId = 0;
        return nextObjectId;
    }

    private void SetObjectIds()
    {
        WorldObject[] worldObjects = GameObject.FindObjectsOfType(typeof(WorldObject)) as WorldObject[];
        foreach (WorldObject worldObject in worldObjects)
        {
            worldObject.ObjectId = nextObjectId++;
            if (nextObjectId >= int.MaxValue) nextObjectId = 0;
        }

        EventObject[] eventObjects = GameObject.FindObjectsOfType(typeof(EventObject)) as EventObject[];
        foreach (EventObject eventObject in eventObjects)
        {
            eventObject.ObjectId = nextObjectId++;
            if (nextObjectId >= int.MaxValue) nextObjectId = 0;
        }
    }

    public void LoadNewScene(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LevelLoading");
    }

    public void LoadSavedScene(string sceneName)
    {
        ResourceManager.LevelName = sceneName;
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LevelLoading");
    }

    IEnumerator LoadNewSceneAsync(string sceneName)
    {
        if (ResourceManager.LevelName != null && ResourceManager.LevelName != "")
        {
            new Thread(() =>
            {
                LoadSavedAssetsAsync();
            }).Start();

            while (gameData == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    void LoadSavedAssetsAsync ()
    {
        // since now we have only 1 save slot, level name does not matter
        gameData = LoadManager.LoadGameData(ResourceManager.LevelName + PersistanceConstants.SAVE_FILENAME_POSTFIX);
    }
}
