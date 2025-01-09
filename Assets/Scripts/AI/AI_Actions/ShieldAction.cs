using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShieldAction : AIAction
{
    [SerializeField] private float chaseSpeedModifier, windupTime, cooldownTime, cancelShieldDistance, maxRaisedTime, minRaisedTime, minChaseDistance, turnaroundTime;
    public Health health;
    [SerializeField] private Animator animator;
    private bool isShieldRaised = false, isOnWindup = false, isOnCooldown = false, attemptingTurnaround = false;
    private float startTimestamp, cooldownTimestamp, raisedTime, turnaroundTimestamp;
    [SerializeField] private Transform target;

    public override void Do()
    {
        if (isOnWindup)
        {
            if (startTimestamp + windupTime < Time.time)
            {
                isOnWindup = false;
                isShieldRaised = true;
                raisedTime = Random.Range(minRaisedTime, maxRaisedTime);
            }
        }
        else if (isOnCooldown)
        {
            if (cooldownTimestamp + cooldownTime < Time.time)
            {
                isOnCooldown = false;
            }
        }
        else if (isShieldRaised)
        {
            if (raisedTime + windupTime + startTimestamp < Time.time || Vector2.Distance(transform.position, target.position) > cancelShieldDistance)
            {
                movement.Move(0);
                isShieldRaised = false;
                isOnCooldown = true;
            }
            else
            {
                float moveDirection = Math.Sign(target.position.x - transform.position.x) * chaseSpeedModifier;
                if (Math.Sign(moveDirection) != Math.Sign(movement.facing.x))
                {
                    if (!attemptingTurnaround)
                    {
                        attemptingTurnaround = true;
                        turnaroundTime = Time.time + turnaroundTimestamp;
                    }
                    else if (turnaroundTime < Time.time)
                    {
                        attemptingTurnaround = false;
                        movement.Move(moveDirection);
                    }
                }
                else if (Vector2.Distance(transform.position, target.position) > minChaseDistance)
                {
                    movement.Move(moveDirection);
                }

            }
        }
        else
        {
            movement.Move(0);
            isOnWindup = true;
        }
    }

    public override bool Stuck()
    {
        return isOnWindup || isOnCooldown || isShieldRaised;
    }
}