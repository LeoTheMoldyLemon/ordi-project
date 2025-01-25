using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class EntityLoader : SaveableBehaviour
{
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }
    public override string Save()
    {
        Dictionary<string, string> saveData = new()
        {
            { "Transform", JsonUtility.ToJson(new SerializableTransform(transform)) },
            { "Health", JsonUtility.ToJson(new SerializableHealth(health)) }
        };
        return JsonConvert.SerializeObject(saveData);
    }

    public override void Load(string jsonData)
    {
        Dictionary<string, string> loadData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
        loadData.TryGetValue("Health", out string healthData);
        loadData.TryGetValue("Transform", out string transformData);
        JsonUtility.FromJson<SerializableTransform>(transformData).Update(transform);
        JsonUtility.FromJson<SerializableHealth>(healthData).Update(health);
    }

}