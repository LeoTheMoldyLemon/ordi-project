using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowFighterAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, attackAction, lostTargetAction;
    [SerializeField] private TextWriter writer;

    void Start()
    {
        if (writer)
            detector.targetDetected.AddListener(() => { if (enabled) writer.Write("Hey! Stop right there!"); });
    }
    protected override AIAction SelectAction()
    {
        if (isTargetDetected)
        {
            return attackAction;
        }
        else if (isTargetLost)
        {
            if (Time.time > timeAtTargetLost + forgetTargetDelay) isTargetLost = false;
            return lostTargetAction;
        }
        else
        {
            return idleAction;
        }

    }

}
