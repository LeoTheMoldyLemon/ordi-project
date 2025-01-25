using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShieldAction : AIAction
{
    [SerializeField] private float windupTime, cooldownTime, cancelShieldDistance, maxRaisedTime, minRaisedTime, turnaroundTime;
    public Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [Header("Debug values")]
    [SerializeField] private bool isShieldRaised = false;
    [SerializeField] private bool isOnWindup = false, isOnCooldown = false, attemptingTurnaround = false;
    private float startTimestamp, cooldownTimestamp, raisedTime, turnaroundTimestamp;

    public void Start()
    {
        health.AddCheckInvincibilityFunctions(CheckIsInvincible);
    }

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
                animator.SetBool("ShieldRaised", false);
            }
        }
        else if (isShieldRaised)
        {
            if (raisedTime + windupTime + startTimestamp < Time.time || Vector2.Distance(transform.position, target.position) > cancelShieldDistance)
            {
                movement.Move(0);
                isShieldRaised = false;
                isOnCooldown = true;
                animator.SetTrigger("LowerShield");
            }
            else
            {
                float moveDirection = Math.Sign(target.position.x - transform.position.x);
                if (Math.Sign(moveDirection) != Math.Sign(movement.facing.x))
                {
                    if (!attemptingTurnaround)
                    {
                        attemptingTurnaround = true;
                        turnaroundTimestamp = Time.time + turnaroundTime;
                    }
                    else if (turnaroundTimestamp < Time.time)
                    {
                        attemptingTurnaround = false;
                        movement.Move(moveDirection);
                        movement.Move(0);
                    }
                }

            }
        }
        else
        {
            movement.Move(0);
            startTimestamp = Time.time;
            isOnWindup = true;
            animator.SetBool("ShieldRaised", true);
            animator.SetTrigger("RaiseShield");
        }
    }

    public bool CheckIsInvincible(Damage damage)
    {
        return isShieldRaised && Math.Sign(damage.transform.position.x - transform.position.x) == Math.Sign(movement.facing.x) && damage.type != Damage.DamageType.ENVIRONMENTAL;
    }

    public override bool Stuck()
    {
        return isOnWindup || isOnCooldown || isShieldRaised;
    }

    public override void Interrupt()
    {
        movement.Move(0);
        isOnWindup = false;
        isShieldRaised = false;
        isOnCooldown = false;
        attemptingTurnaround = false;
    }
}