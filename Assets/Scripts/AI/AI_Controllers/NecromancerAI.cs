using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, reviveAction, shieldAction;
    [SerializeField] private Health[] targets;

    void Start()
    {
        writer.Write("Die.");
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

}
