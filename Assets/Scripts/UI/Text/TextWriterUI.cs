using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TextWriterUI : MonoBehaviour
{
    [SerializeField] private TextWriter writer;
    [SerializeField] private InputActionReference textInput;
    [SerializeField] private TextMeshProUGUI textMesh;
    public static TextWriterUI Instance { get; private set; }

    public UnityEvent textDone;

    public bool textSkippable = false;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    void Start()
    {
        writer.textWritten.AddListener(() => textSkippable = true);
        writer.textRemoved.AddListener(Clear);
        textInput.action.performed += (_) => { if (textSkippable) Clear(); };
    }
    public void Write(string text, Color color)
    {
        transform.localScale = new Vector3(1, 1, 1);
        Debug.Log(color);
        textMesh.color = color;
        writer.Write(text);
    }

    private void Clear()
    {
        textSkippable = false;
        transform.localScale = new Vector3(0, 0, 0);
        writer.Clear();
        textDone.Invoke();
    }


}
