using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAction : AIAction
{
    [SerializeField] private float windupTime, cooldownTime;
    [SerializeField] private bool isOnWindup = false, isOnCooldown = false;
    [SerializeField] public Health[] targets;
    [SerializeField] public Animator animator;
    private Coroutine reviveCoroutine;

    public override void Do()
    {
        if (!isOnWindup && !isOnCooldown)
            reviveCoroutine = StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        Debug.Log("Starting revive");
        animator.SetTrigger("ReviveAction");
        isOnWindup = true;

        yield return new WaitForSeconds(windupTime);

        Debug.Log("Reviving targets");
        foreach (var target in targets)
            target.Revive();
        isOnCooldown = true;
        isOnWindup = false;

        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
    }

    public override bool Stuck()
    {
        return isOnWindup || isOnCooldown;
    }

    public override void Interrupt()
    {
        isOnWindup = false;
        if (reviveCoroutine != null) StopCoroutine(reviveCoroutine);
    }
}