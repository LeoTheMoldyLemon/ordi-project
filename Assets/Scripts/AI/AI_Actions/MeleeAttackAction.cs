using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : AIAction
{
    [SerializeField] private float chaseSpeedModifier, targetAttackDistance;
    public Attack attack;
    [SerializeField] private Transform target;

    public override void Do()
    {
        if (attack.isOnWindup || attack.isOnCooldown || !target)
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
            movement.Move(chaseSpeedModifier);
        else
            movement.Move(-chaseSpeedModifier);
    }

    public override bool Stuck()
    {
        return attack.isOnWindup || attack.isOnCooldown;
    }
}