using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private Dictionary<string, SaveableBehaviour> saveableObjects = new();
    [SerializeField] private string saveFileName;

    [SerializeField] private Checkpoint currentCheckpoint;
    [SerializeField] private CameraFader cameraFader;
    [SerializeField] private bool debugMode = false;

    void Awake()
    {
        if (!Instance) Instance = this;
    }

    public void Start()
    {
        foreach (SaveableBehaviour saveableObject in (SaveableBehaviour[])FindObjectsOfType(typeof(SaveableBehaviour)))
        {
            Debug.Log("Registered saveable object " + saveableObject.name + "(" + saveableObject.GUID + ")");
            saveableObjects.Add(saveableObject.GUID, saveableObject);
        }
        if (!debugMode)
            Load();
        if (cameraFader)
            cameraFader.FadeIn();
    }

    public void Save(Checkpoint checkpoint)
    {
        Debug.Log("Saving checkpoint...");
        if (currentCheckpoint)
            currentCheckpoint.Deactivate();
        currentCheckpoint = checkpoint;
        currentCheckpoint.Activate();

        Dictionary<string, string> saveData = new();
        foreach (KeyValuePair<string, SaveableBehaviour> keyValuePair in saveableObjects)
        {
            SaveableBehaviour saveableObject = keyValuePair.Value;
            string guid = keyValuePair.Key;
            Debug.Log("Saving " + saveableObject.name + "(" + guid + ")");
            saveData.Add(guid, saveableObject.Save());
        }

        string serializedData = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), serializedData);
    }

    public void Load()
    {
        Debug.Log("Loading checkpoint...");
        string serializedData = File.ReadAllText(Path.Combine(Application.persistentDataPath, saveFileName));
        Dictionary<string, string> saveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedData);

        foreach (KeyValuePair<string, string> keyValuePair in saveData)
        {
            string guid = keyValuePair.Key, data = keyValuePair.Value;
            try
            {
                saveableObjects.TryGetValue(guid, out SaveableBehaviour saveableObject);
                saveableObject.Load(data);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load object with GUID " + guid + "\n" + e.Message);
                continue;
            }
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        if (cameraFader)
        {
            cameraFader.FadeOut();
            yield return new WaitForSeconds(cameraFader.fadeOutTime);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}