using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private string saveFileName;
    void Start()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, saveFileName)))
            continueButton.interactable = false;
    }

    public void OnContinue()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnNewGame()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, saveFileName)))
            File.Delete(Path.Combine(Application.persistentDataPath, saveFileName));
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnSettings()
    {
        Instantiate(settingsPrefab);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
