using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource SFXSource, musicSource1, musicSource2;
    AudioSource activeSource, nextSource;
    public AudioMixer audioMixer;


    private MusicPlayer currentPlayer, nextPlayer;


    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    void Start()
    {
        SetVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0.8f));
        SetVolume("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0.8f));
        SetVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0.8f));
    }

    public void SetVolume(string variableName, float volume)
    {
        if (volume == 0) volume = 0.0001f;
        audioMixer.SetFloat(variableName, Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat(variableName, volume);
    }
    public float GetVolume(string variableName)
    {
        audioMixer.GetFloat(variableName, out float volume);
        return Mathf.Pow(10, volume / 20f);
    }


    public void PlaySFX(AudioClip clip, float volume, Vector3 position)
    {
        AudioSource audioSource = Instantiate(SFXSource, position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, clip.length);
    }
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        PlaySFX(clip, volume, Camera.main.transform.position);
    }



    public float PlayNext(AudioClip clip, bool loop)
    {
        if (musicSource1.isPlaying)
        {
            activeSource = musicSource1;
            nextSource = musicSource2;
        }
        else
        {
            activeSource = musicSource2;
            nextSource = musicSource1;
        }

        if (activeSource.isPlaying)
        {
            nextSource.Stop();
            nextSource.clip = clip;
            nextSource.loop = loop;
            activeSource.loop = false;
            nextSource.PlayDelayed(activeSource.clip.length - activeSource.time);
            return activeSource.clip.length - activeSource.time;
        }
        else
        {
            activeSource.loop = loop;
            activeSource.clip = clip;
            activeSource.Play();
            return 0;
        }

    }

}
