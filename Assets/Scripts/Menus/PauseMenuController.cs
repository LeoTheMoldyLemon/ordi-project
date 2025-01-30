using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public string mainMenuSceneName;
    public GameObject settingsPrefab;

    void Start()
    {
        Time.timeScale = 0;
    }
    public void onQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
    public void onSettings()
    {
        Instantiate(settingsPrefab);
    }

    public void onResume()
    {
        Time.timeScale = 1;
        PlayerController.Instance.paused = false;
        Destroy(gameObject);
    }
}
