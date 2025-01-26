using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public Slider slider;
    public string variableName;
    void Start()
    {
        slider.value = AudioManager.Instance.GetVolume(variableName);
    }

    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetVolume(variableName, volume);
    }
}
