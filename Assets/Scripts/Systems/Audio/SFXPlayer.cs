using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private float volume = 0;
    public void Play(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Clip missing on " + name);
            return;
        }
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(clip, volume, transform.position);
    }
    public void PlayRandomClip(AudioClip[] clips)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, transform.position);
    }
    public void PlayRandom(SFXBundle bundle)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(bundle.clips[Random.Range(0, bundle.clips.Length)], volume, transform.position);
    }

}
