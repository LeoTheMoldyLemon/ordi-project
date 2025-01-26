using System;
using UnityEditor;
using UnityEngine;

public class DoorLoader : SaveableBehaviour
{
    private DoorController doorController;

    public void Awake()
    {
        doorController = GetComponent<DoorController>();
    }
    public override string Save()
    {
        return JsonUtility.ToJson(new SerializableDoor(doorController));
    }

    public override void Load(string jsonData)
    {
        JsonUtility.FromJson<SerializableDoor>(jsonData).Update(doorController);
    }

}