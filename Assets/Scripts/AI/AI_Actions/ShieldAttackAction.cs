using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShieldAttackAction : AIAction
{
    [SerializeField] private float windupTime, cooldownTime;
    [SerializeField] private bool isShieldTwoSided = false;
    [SerializeField] private AIAction action;
    public Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [Header("Debug values")]
    [SerializeField] private bool isShieldRaised = false;
    [SerializeField] private bool isOnWindup = false;
    private float startTimestamp;

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
            }
        }
        else if (isShieldRaised)
        {
            action.Do();
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
        return isShieldRaised &&
            (isShieldTwoSided || Math.Sign(damage.transform.position.x - transform.position.x) == Math.Sign(movement.facing.x)) && damage.type != Damage.DamageType.ENVIRONMENTAL;
    }

    public override bool Stuck()
    {
        return isOnWindup;
    }

    public override void Interrupt()
    {
        movement.Move(0);
        isOnWindup = false;
        isShieldRaised = false;
        animator.SetBool("ShieldRaised", false);
        animator.SetTrigger("LowerShield");
    }
}