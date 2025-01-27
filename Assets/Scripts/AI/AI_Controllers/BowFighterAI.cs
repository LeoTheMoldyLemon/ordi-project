using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowFighterAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, attackAction, lostTargetAction;

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
