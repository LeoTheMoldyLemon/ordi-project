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

        transform.localScale = new Vector3(scale.x / transform.parent.transform.localScale.x, scale.y / transform.parent.transform.localScale.y, scale.z / transform.parent.transform.localScale.z);
    }

    public void Write(string text, float speed)
    {
        textMesh.text = "";
        if (currentWrittingCoroutine != null) StopCoroutine(currentWrittingCoroutine);
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
            textMesh.text += word + " ";
            yield return new WaitForSeconds(1 / (baseWritingSpeed * speed));
        }
        textWritten.Invoke();
        yield return new WaitForSeconds(baseTextDuration / speed);
        textMesh.text = "";
        textRemoved.Invoke();
    }


}
