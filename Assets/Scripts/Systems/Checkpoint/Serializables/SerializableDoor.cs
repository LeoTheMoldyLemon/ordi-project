using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableDoor
{
    public bool isOpen;

    public SerializableDoor(DoorController door)
    {
        isOpen = door.isOpen;
    }

    public void Update(DoorController door)
    {
        if (isOpen)
            door.OpenDoor();

    }
}