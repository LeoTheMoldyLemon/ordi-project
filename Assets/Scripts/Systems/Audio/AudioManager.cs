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

    public Coroutine musicCoroutine;

    private MusicPlayer currentPlayer, nextPlayer;

    private bool musicExiting = false, musicStarting = false;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    void Start()
    {
        MusicPlayer.changedPlayer.AddListener(ChangeMusic);
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

    void ChangeMusic()
    {
        if ((currentPlayer == null || currentPlayer != nextPlayer || nextPlayer != MusicPlayer.queue.Max) && !musicStarting)
        {
            Debug.Log("Changing next player...");
            nextPlayer = MusicPlayer.queue.Max;
            musicCoroutine = StartCoroutine(FadeAndPlay());
        }
    }


    private float PlayNext(AudioClip clip, bool loop)
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

    private void SetMusicVolume(float volume)
    {
        musicSource1.volume = volume;
        musicSource2.volume = volume;
    }

    private IEnumerator FadeAndPlay()
    {
        Debug.Log("fade and play");
        if (musicExiting) yield break;
        Debug.Log("proceeding");
        if (currentPlayer != null)
        {
            Debug.Log("exiting");
            musicExiting = true;
            if (currentPlayer.endClip)
            {
                Debug.Log("started exit clip");
                SetMusicVolume(1);
                float delay = PlayNext(currentPlayer.endClip, false);
                yield return new WaitForSeconds(currentPlayer.endClip.length + delay);
            }
            else
            {
                Debug.Log("started exit fade");
                Debug.Log(currentPlayer.fadeTime);
                if (currentPlayer.fadeTime != 0)
                {
                    float fadeStep = activeSource.volume / currentPlayer.fadeTime * Time.deltaTime;
                    while (activeSource.volume > fadeStep)
                    {
                        SetMusicVolume(activeSource.volume - fadeStep);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }
            }
            SetMusicVolume(0);
            activeSource.Stop();
            nextSource.Stop();
            musicExiting = false;
            Debug.Log("exited");
        }
        currentPlayer = nextPlayer;
        if (currentPlayer == null) yield break;

        Debug.Log("starting");
        musicStarting = true;
        if (currentPlayer.startClip != null)
        {
            SetMusicVolume(1);
            PlayNext(currentPlayer.startClip, false);
            yield return new WaitForSeconds(1f);
        }
        if (currentPlayer.loopClip != null)
        {
            SetMusicVolume(1);
            PlayNext(currentPlayer.loopClip, true);
        }
        if (currentPlayer.startClip != null) yield return new WaitForSeconds(currentPlayer.startClip.length);
        musicStarting = false;
        ChangeMusic();
    }
}
