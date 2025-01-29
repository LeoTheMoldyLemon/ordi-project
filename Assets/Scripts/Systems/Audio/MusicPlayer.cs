using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{

    public HashSet<AIBehaviour> enemiesInCombat = new();
    public bool bossFight;
    public float clipSwitchTime;

    public AudioClip combatMusic, bossMusic, idleMusic1, idleMusic2;

    private AudioClip nextIdleClip;

    public static MusicPlayer Instance { get; private set; }

    void Awake()
    {
        if (!Instance)
            Instance = this;
        nextIdleClip = idleMusic1;
    }

    void Update()
    {
        if (Time.time > clipSwitchTime)
        {
            AudioClip nextClip;
            if (bossFight)
                nextClip = bossMusic;
            else if (enemiesInCombat.Count > 0) nextClip = combatMusic;
            else
            {
                nextClip = nextIdleClip;
                if (nextIdleClip == idleMusic1)
                    nextIdleClip = idleMusic2;
                else
                    nextIdleClip = idleMusic1;
            }

            Debug.Log("Playing next: " + nextClip.name);
            clipSwitchTime = AudioManager.Instance.PlayNext(nextClip, true) - 1 + nextClip.length + Time.time;
        }
    }
}
