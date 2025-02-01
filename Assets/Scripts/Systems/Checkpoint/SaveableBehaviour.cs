using System;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

[ExecuteAlways]
public abstract class SaveableBehaviour : MonoBehaviour
{
    public abstract string Save();
    public abstract void Load(string jsonData);

    protected virtual void OnEnable()
    {
        Debug.Log(name + ": " + guid);
#if UNITY_EDITOR
        if (!Application.isPlaying && guid == "")
        {
            Undo.RecordObject(this, "Set GUID");
            guid = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("set" + name + " " + guid, this);
        }
#endif
    }
    public string guid = "";

}