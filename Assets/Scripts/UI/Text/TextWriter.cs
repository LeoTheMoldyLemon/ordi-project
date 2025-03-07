using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextWriter : MonoBehaviour
{
    [SerializeField] public float baseWritingSpeed = 5;
    [SerializeField] public float baseTextDuration = 5;

    public UnityEvent textWritten, textRemoved;
    private TMP_Text textMesh;
    private Vector3 scale;

    private Coroutine currentWrittingCoroutine;

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }
    void Start()
    {
        scale = transform.localScale;
    }
    void Update()
    {
        var parentScale = transform.parent.transform.localScale;
        if (parentScale.x == 0 | parentScale.y == 0 | parentScale.z == 0)
            transform.localScale = Vector3.zero;
        else
            transform.localScale = new Vector3(scale.x / parentScale.x, scale.y / parentScale.y, scale.y / parentScale.z);
    }

    public void Write(string text, float speed)
    {
        Debug.Log("Text writting: " + text);
        Clear();
        currentWrittingCoroutine = StartCoroutine(Typewriter(text, speed));
    }

    public void Write(string text)
    {
        Write(text, 1);
    }

    public void Clear()
    {
        if (currentWrittingCoroutine != null) StopCoroutine(currentWrittingCoroutine);
        textMesh.text = "";
    }

    private IEnumerator Typewriter(string text, float speed)
    {
        foreach (string word in text.Split(" "))
        {
            Debug.Log("Word writting: " + word);
            textMesh.text += word + " ";
            yield return new WaitForSecondsRealtime(1 / (baseWritingSpeed * speed));
        }
        textWritten.Invoke();
        yield return new WaitForSecondsRealtime(baseTextDuration / speed);
        textMesh.text = "";
        textRemoved.Invoke();
    }


}
