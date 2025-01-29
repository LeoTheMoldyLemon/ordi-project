using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdFighterAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, attackAction, dashAttackAction, lostTargetAction;
    [SerializeField] private float dashMinTargetDistance, dashMaxTargetDistance;
    [SerializeField] private float dashTargetDistance;

    protected override AIAction SelectAction()
    {
        if (isTargetDetected)
        {
            if (Vector2.Distance(transform.position, detector.target.position) < dashTargetDistance)
                return attackAction;
            else
            {
                dashTargetDistance = Random.Range(dashMinTargetDistance, dashMaxTargetDistance);
                return dashAttackAction;
            }
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
