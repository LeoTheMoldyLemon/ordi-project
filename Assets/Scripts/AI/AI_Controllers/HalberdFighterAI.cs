using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdFighterAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, attackAction, dashAttackAction, lostTargetAction;
    [SerializeField] private float dashMinTargetDistance;

    protected override AIAction SelectAction()
    {
        if (isTargetDetected)
        {
            if (Vector2.Distance(transform.position, detector.target.position) < dashMinTargetDistance)
                return attackAction;
            else
                return dashAttackAction;
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
