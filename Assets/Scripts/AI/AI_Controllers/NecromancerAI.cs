using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NecromancerAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, reviveAction, shieldAction;
    [SerializeField] private Health[] targets;
    [SerializeField] private string deathText;
    [SerializeField] private string endGameSceneName;

    void Start()
    {
        writer.Write("Die.");
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.bossFight = true;
        health.death.AddListener(() =>
        {
            writer.baseWritingSpeed = 0.7f;
            writer.Write(deathText);
            Invoke(nameof(EndGame), 10);
        });
    }
    protected override AIAction SelectAction()
    {
        if (Array.Find(targets, (target) => target.currentHealth != 0) == null)
        {
            if (currentAction == shieldAction)
                shieldAction.Interrupt();
            return reviveAction;
        }
        if (isTargetDetected)
        {
            return shieldAction;
        }
        else
        {
            return idleAction;
        }

    }

    public void EndGame()
    {
        SceneManager.LoadScene(endGameSceneName);
    }

}
