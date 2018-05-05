using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {

    public string sceneName;

	public void OnSelect ()
    {
        SceneManager.LoadScene(sceneName);
    } 
}
