using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioPlayer : MonoBehaviour
{
    public void Play(AudioClip clip, float volume = 1f)
    {
        AudioPlayer.Instance.PlaySFX(clip, volume, transform.position);
    }
    public void PlayRandom(AudioClip[] clips, float volume = 1f)
    {
        AudioPlayer.Instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, transform.position);
    }

}
