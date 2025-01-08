using System;
using UnityEditor;
using UnityEngine;

public class TransformLoader : SaveableBehaviour
{
    public override string Save()
    {
        return JsonUtility.ToJson(new SerializeableTransform(transform));
    }

    public override void Load(string jsonData)
    {
        JsonUtility.FromJson<SerializeableTransform>(jsonData).updateTransform(transform);
    }

    [Serializable]
    class SerializeableTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public SerializeableTransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        public void updateTransform(Transform transform)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }
}