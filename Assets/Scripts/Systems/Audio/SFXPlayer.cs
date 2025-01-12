using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void Play(AudioClip clip, float volume)
    {
        AudioManager.Instance.PlaySFX(clip, volume, transform.position);
    }
    public void PlayRandom(AudioClip[] clips, float volume)
    {
        AudioManager.Instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, transform.position);
    }

}
