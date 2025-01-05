using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AIAction
{
    [SerializeField] private float chaseSpeedPercent, targetAttackDistance;
    [SerializeField] private Transform target;

    public override void Do()
    {
        if (attack.isAttacking || attack.isOnCooldown)
        {
            movement.Move(0);
        }
        else if (Vector2.Distance(transform.position, target.position) < targetAttackDistance)
        {
            if (movement.facing.x < 0 && target.position.x - transform.position.x > 0)
                movement.Move(1);
            else if (movement.facing.x > 0 && target.position.x - transform.position.x < 0)
                movement.Move(-1);
            else
            {
                movement.Move(0);
                attack.Perform();
            }
        }
        else if (target.position.x > transform.position.x)
            movement.Move(chaseSpeedPercent);
        else
            movement.Move(-chaseSpeedPercent);
    }
}