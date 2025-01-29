using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    [SerializeField] protected Detector detector;
    [SerializeField] protected Health health;
    [SerializeField] protected float forgetTargetDelay;
    [SerializeField] protected bool isTargetDetected = false;
    [SerializeField] protected bool isTargetLost = false;
    [SerializeField] protected bool firstDetectionPassed = false;
    [SerializeField] private string[] onDetectText, onDeathText;
    [SerializeField] protected TextWriter writer;

    [SerializeField] protected AIAction currentAction;
    protected float timeAtTargetLost;
    void Awake()
    {
        detector.targetDetected.AddListener(TargetDetectedHandler);
        detector.targetLost.AddListener(TargetLostHandler);
        health.death.AddListener(() =>
        {
            Debug.Log("Stopping cause dead");
            if (onDeathText.Length != 0)
                writer.Write(onDeathText[Random.Range(0, onDetectText.Length)]);
            if (currentAction != null)
                currentAction.Interrupt();
            currentAction = null;
            enabled = false;
            if (MusicPlayer.Instance != null)
                MusicPlayer.Instance.enemiesInCombat.Remove(this);
        });
        health.revival.AddListener(() =>
        {
            Debug.Log("Starting again because revived");
            enabled = true;
        });
    }

    void Start()
    {


    }

    void Update()
    {
        if (currentAction == null || !currentAction.Stuck())
            currentAction = SelectAction();
        currentAction.Do();
    }

    protected abstract AIAction SelectAction();


    private void TargetDetectedHandler()
    {
        if (!firstDetectionPassed)
        {
            firstDetectionPassed = true;
            if (onDetectText.Length != 0)
                writer.Write(onDetectText[Random.Range(0, onDetectText.Length)]);
        }
        isTargetDetected = true;
        isTargetLost = false;
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.enemiesInCombat.Add(this);
    }
    private void TargetLostHandler()
    {
        isTargetDetected = false;
        isTargetLost = true;
        timeAtTargetLost = Time.time;
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.enemiesInCombat.Remove(this);
    }
}
