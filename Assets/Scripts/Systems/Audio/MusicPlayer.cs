using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour, IComparable<MusicPlayer>
{
    public AudioClip startClip, loopClip, endClip;
    public float fadeTime = 1;
    public int priority = 0;
    public static SortedSet<MusicPlayer> queue = new();
    private bool active = false;
    public static UnityEvent changedPlayer = new();


    void OnTriggerEnter2D(Collider2D collision)
    {
        Activate();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Deactivate();
    }

    public void Activate()
    {
        if (!active)
        {
            active = true;
            queue.Add(this);
            changedPlayer.Invoke();
        }
    }
    public void Deactivate()
    {
        if (active)
        {
            active = false;
            queue.Remove(this);
            changedPlayer.Invoke();
        }
    }

    public int CompareTo(MusicPlayer other)
    {
        return priority - other.priority;
    }
}
