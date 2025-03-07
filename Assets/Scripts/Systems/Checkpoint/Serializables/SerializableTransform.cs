using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public SerializableTransform(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    public void Update(Transform transform)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
    }
}