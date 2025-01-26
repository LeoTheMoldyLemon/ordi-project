using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public Dictionary<string, MusicTrack> tracks = new();

    private MusicTrack currentTrack = null;
    public MusicTrack nextTrack = null;

    enum TrackProgress
    {
        NONE, START, MAIN, END
    }

    private TrackProgress trackProgress = TrackProgress.NONE;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        foreach (MusicTrack track in GetComponentsInChildren<MusicTrack>())
            tracks.Add(track.gameObject.name, track);

    }

    private void NextClip()
    {
        if (trackProgress == TrackProgress.START)
        {
            trackProgress = TrackProgress.MAIN;
            PlayClip(currentTrack.main);
        }
        else if (currentTrack == nextTrack)
        {
            if (currentTrack == null)
            {
                trackProgress = TrackProgress.NONE;
            }
            else if (trackProgress == TrackProgress.MAIN)
            {
                PlayClip(currentTrack.main);
            }
        }
        else
        {
            if (trackProgress == TrackProgress.MAIN)
            {
                trackProgress = TrackProgress.END;
                PlayClip(currentTrack.end);
            }
            else if (nextTrack == null)
            {
                trackProgress = TrackProgress.NONE;
                currentTrack = nextTrack;
            }
            else
            {
                currentTrack = nextTrack;
                trackProgress = TrackProgress.START;
                PlayClip(currentTrack.start);
            }
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            NextClip();
        }
        audioSource.clip = clip;
        audioSource.Play();
        Invoke(nameof(NextClip), clip.length);
    }

    public void Play(string nameOfTrack)
    {
        nextTrack = tracks[nameOfTrack];
        if (currentTrack == null) NextClip();
    }
}
