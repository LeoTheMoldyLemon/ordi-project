using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private float volume = 0;
    public void Play(AudioClip clip)
    {
        AudioManager.Instance.PlaySFX(clip, volume, transform.position);
    }
    public void PlayRandom(AudioClip[] clips)
    {
        AudioManager.Instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, transform.position);
    }

}
