using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    [SerializeField] protected Detector detector;
    [SerializeField] protected float forgetTargetDelay;
    [SerializeField] protected bool isTargetDetected = false;
    [SerializeField] protected bool isTargetLost = false;

    [SerializeField] protected AIAction currentAction;
    protected float timeAtTargetLost;
    void Awake()
    {
        detector.targetDetected.AddListener(TargetDetectedHandler);
        detector.targetLost.AddListener(TargetLostHandler);
    }

    void Update()
    {
        if (currentAction == null || !currentAction.Stuck())
            currentAction = SelectAction();
        currentAction.Do();
    }

    protected abstract AIAction SelectAction();


    public void TargetDetectedHandler()
    {
        isTargetDetected = true;
        isTargetLost = false;
    }
    public void TargetLostHandler()
    {
        isTargetDetected = false;
        isTargetLost = true;
        timeAtTargetLost = Time.time;
    }
}
