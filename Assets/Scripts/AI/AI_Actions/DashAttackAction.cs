using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashAttackAction : AIAction
{
    [SerializeField] private float dashSpeedModifier, dashWindup, maxDashDistance;
    [SerializeField] private Transform target;
    private List<Collider2D> collisions = new();
    public Attack attack;
    public Animator animator;

    [SerializeField] public bool dashing = false, windingUp = false;

    public override void Do()
    {
        if (dashing)
        {
            if (collisions.Count > 0 || (target.transform.position.x - transform.position.x) * movement.facing.x < maxDashDistance)
            {
                attack.Perform();
                dashing = false;
                animator.SetBool("Dashing", false);
                movement.Move(0);
            }
        }
        else
        {
            if (!attack.isAttacking && !attack.isOnCooldown && !attack.isOnWindup && !windingUp)
            {
                if (target.position.x - transform.position.x > 0)
                    StartCoroutine(StartDash(1));
                else
                    StartCoroutine(StartDash(-1));
            }

        }
    }

    private IEnumerator StartDash(float dashDirection)
    {
        windingUp = true;
        animator.SetBool("Dashing", true);
        animator.SetTrigger("DashWindup");

        movement.Move(dashDirection);
        movement.Move(0);


        yield return new WaitForSeconds(dashWindup);

        dashing = true;
        windingUp = false;
        animator.SetTrigger("DashStart");
        movement.Move(dashDirection * dashSpeedModifier);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collisions.Add(collision);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        collisions.Remove(collision);
    }


    public override bool Stuck()
    {
        return windingUp || dashing || attack.isAttacking || attack.isOnCooldown || attack.isOnWindup;
    }

    public override void Interrupt()
    {
        movement.Move(0);
        attack.Cancel();
    }
}