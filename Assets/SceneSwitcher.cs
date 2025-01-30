using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public float time;
    void Start()
    {
        Invoke(nameof(SwitchScene), time);
    }
    private void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
