using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static bool created = false;
    private bool initialised = false;
    private HUD hud;

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
        if (initialised)
        {
            LoadDetails();
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (initialised)
        {
            LoadDetails();
        }
    }

    private void LoadDetails()
    {
        Player[] players = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
        foreach (Player player in players)
        {
            if (player.human) hud = player.GetComponentInChildren<HUD>();
        }
    }
}
