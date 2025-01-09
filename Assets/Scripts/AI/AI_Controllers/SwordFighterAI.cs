using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFighterAI : AIBehaviour
{

    [SerializeField] private AIAction idleAction, attackAction, shieldAction, lostTargetAction;
    [SerializeField] private float minShieldDownTime, maxShieldDownTime;
    [SerializeField] private float minShieldUpDistance;
    private float raiseShieldTimestamp;

    protected override AIAction SelectAction()
    {
        if (isTargetDetected)
        {
            if (Vector2.Distance(transform.position, detector.target.position) < minShieldUpDistance)
                return attackAction;
            else
            {
                if (raiseShieldTimestamp < Time.time)
                {
                    raiseShieldTimestamp = Time.time + Random.Range(minShieldDownTime, maxShieldDownTime);
                    return shieldAction;
                }
                else
                {
                    return attackAction;
                }
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
