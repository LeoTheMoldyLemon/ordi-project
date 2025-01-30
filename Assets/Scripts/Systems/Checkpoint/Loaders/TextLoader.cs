using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextLoader : SaveableBehaviour
{

    public static TextLoader Instance { get; private set; }
    public bool active = false;
    public string[] text = new string[]{
        "Wow that didn't take very long.",
        "I guess I overestimated you.",
        "You are less useful than I thought you would be.",
        "…Wow.",
        "Here's a tip: have you tried not dying?",
        "You are lucky I don't have another sack of bones to revive.",
        "Useless…",
    };
    void Awake()
    {
        if (!Instance) Instance = this;
    }

    public override string Save()
    {
        return JsonUtility.ToJson(new Data()
        {
            active = active
        });
    }

    public override void Load(string jsonData)
    {
        active = JsonUtility.FromJson<Data>(jsonData).active;
        if (active)
            TextWriterUI.Instance.Write(text[Random.Range(0, text.Length)], new Color(0.298f, 0.820f, 0.745f, 1f));

    }

    class Data
    {
        public bool active;
    }
}