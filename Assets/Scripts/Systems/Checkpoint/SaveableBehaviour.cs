using System;
using UnityEditor;
using UnityEngine;

public abstract class SaveableBehaviour : MonoBehaviour
{
    public abstract string Save();
    public abstract void Load(string jsonData);

    private string guid;
    public string GUID
    {
        get
        {
            guid ??= GlobalObjectId.GetGlobalObjectIdSlow(this).ToString();
            return guid;
        }
    }
}