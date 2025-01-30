using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueLine : MonoBehaviour
{
    public Color color;
    public string text;
    public CameraDock dock;

    public virtual void Activate()
    {
        TextWriterUI.Instance.Write(text, color);
        if (dock != null) dock.Activate();
    }

    public virtual void Deactivate()
    {
        if (dock != null) dock.Deactivate();
    }
}
