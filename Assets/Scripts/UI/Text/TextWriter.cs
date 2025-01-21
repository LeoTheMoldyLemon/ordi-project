using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] private float baseWritingSpeed = 5;
    [SerializeField] private float baseTextDuration = 5;
    private TMP_Text textMesh;
    private Vector3 scale;
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        scale = transform.localScale;
    }
    void Update()
    {

        transform.localScale = new Vector3(scale.x / transform.parent.transform.localScale.x, scale.y / transform.parent.transform.localScale.y, scale.z / transform.parent.transform.localScale.z);
    }

    public void Write(string text, float speed)
    {
        StartCoroutine(Typewriter(text, speed));
    }

    public void Write(string text)
    {
        Write(text, 1);
    }

    private IEnumerator Typewriter(string text, float speed)
    {
        textMesh.text = "";
        foreach (string word in text.Split(" "))
        {
            textMesh.text += word + " ";
            yield return new WaitForSeconds(1 / (baseWritingSpeed * speed));
        }
        yield return new WaitForSeconds(baseTextDuration / speed);
        textMesh.text = "";
    }


}
