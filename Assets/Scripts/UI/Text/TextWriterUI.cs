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

    public UnityEvent textDone;

    void Start()
    {
        writer.textWritten.AddListener(Hide);
        textInput.action.performed += (_) => Hide();
    }
    void Update()
    {
        if (textInput.action.ReadValue<float>() > 0) { }
    }

    public void Write(string text, Color color)
    {
        transform.localScale = new Vector3(1, 1, 1);
        textMesh.color = color;
        writer.Write(text);
    }

    private void Hide()
    {
        transform.localScale = new Vector3(0, 0, 0);
        writer.Clear();
        textDone.Invoke();
    }


}
