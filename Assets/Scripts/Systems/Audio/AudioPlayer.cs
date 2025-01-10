using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }
    public AudioSource SFXSource, musicSource;

    void Awake()
    {
        if (!Instance)
            Instance = this;
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

    public void PlayMusic(AudioClip clip, float volume, Vector3 position)
    {
        AudioSource audioSource = Instantiate(musicSource, position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, clip.length);
    }
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        PlayMusic(clip, volume, Camera.main.transform.position);
    }
}
