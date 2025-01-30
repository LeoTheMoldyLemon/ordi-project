using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueLoader : SaveableBehaviour
{
    public TextTrigger textTrigger;

    void Awake()
    {
        textTrigger = GetComponent<TextTrigger>();
    }
    public override string Save()
    {
        Debug.Log(name + " " + textTrigger.triggered);
        return JsonUtility.ToJson(new Data()
        {
            triggered = textTrigger.triggered
        });
    }

    public override void Load(string jsonData)
    {
        Debug.Log("LOADDDEDDDDD. " + name);
        textTrigger.triggered = JsonUtility.FromJson<Data>(jsonData).triggered;

        Debug.Log(name + " " + textTrigger.triggered);
    }

    class Data
    {
        public bool triggered;
    }
}