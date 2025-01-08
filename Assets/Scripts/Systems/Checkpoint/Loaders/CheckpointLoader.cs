using System;
using UnityEditor;
using UnityEngine;

public class CheckpointLoader : SaveableBehaviour
{
    private Checkpoint checkpoint;

    public void Awake()
    {
        checkpoint = GetComponent<Checkpoint>();
    }
    public override string Save()
    {
        return JsonUtility.ToJson(new Data()
        {
            active = checkpoint.Active
        });
    }

    public override void Load(string jsonData)
    {
        if (JsonUtility.FromJson<Data>(jsonData).active)
            checkpoint.Activate();
        else
            checkpoint.Deactivate();
    }

    class Data
    {
        public bool active;
    }
}