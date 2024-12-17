using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighterAI : MonoBehaviour
{
    [SerializeField] private Detector detector;
    [SerializeField] private Attack attack;
    [SerializeField] private Movement movement;


    [SerializeField] private AIAction idleAction, attackAction, lostTargetAction;
    [SerializeField] private float forgetTargetDelay;
    [SerializeField] private bool isTargetDetected = false;
    [SerializeField] private bool isTargetLost = false;
    private float timeAtTargetLost;
    void Awake()
    {
        idleAction.movement = movement;
        attackAction.movement = movement;
        attackAction.attack = attack;
        detector.targetDetected.AddListener(TargetDetectedHandler);
        detector.targetLost.AddListener(TargetLostHandler);
    }

    void Update()
    {
        if (isTargetDetected)
        {
            attackAction.Do();
        }
        else if (isTargetLost)
        {
            lostTargetAction.Do();
            if (Time.time > timeAtTargetLost + forgetTargetDelay) isTargetLost = false;
        }
        else
        {
            idleAction.Do();
        }

    }


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
