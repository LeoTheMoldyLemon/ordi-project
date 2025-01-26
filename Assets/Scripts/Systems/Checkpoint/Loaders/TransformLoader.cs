using System;
using UnityEditor;
using UnityEngine;

public class TransformLoader : SaveableBehaviour
{
    public override string Save()
    {
        return JsonUtility.ToJson(new SerializableTransform(transform));
    }

    public override void Load(string jsonData)
    {
        JsonUtility.FromJson<SerializableTransform>(jsonData).Update(transform);
    }


}
