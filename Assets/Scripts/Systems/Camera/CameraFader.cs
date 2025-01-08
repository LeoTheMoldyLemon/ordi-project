using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFader : MonoBehaviour
{
    private Texture2D texture;
    public Color color = Color.black;
    public float fadeOutTime = 1, fadeInTime = 1;
    public bool fadeingOut = false;

    public float alpha = 0;
    public void Awake()
    {
        texture = new Texture2D(1, 1);
        if (fadeingOut) alpha = 1;
        UpdateTexture();
    }

    public void FadeIn()
    {
        fadeingOut = false;
    }
    public void FadeOut()
    {
        fadeingOut = true;
    }

    public void Update()
    {
        if (fadeingOut && alpha != 1)
        {
            alpha += Time.deltaTime / fadeOutTime;
            if (alpha > 1) alpha = 1;
            UpdateTexture();
        }
        else if (!fadeingOut && alpha != 0)
        {
            alpha -= Time.deltaTime / fadeInTime;
            if (alpha < 0) alpha = 0;
            UpdateTexture();
        }
    }

    public void OnGUI()
    {
        if (alpha != 0) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    private void UpdateTexture()
    {
        texture.SetPixel(0, 0, new Color(color.r, color.g, color.b, alpha * color.a));
        texture.Apply();
    }



}
