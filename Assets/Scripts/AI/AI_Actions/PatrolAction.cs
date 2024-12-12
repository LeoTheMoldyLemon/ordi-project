


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : AIAction
{
    [SerializeField] private Vector2[] patrolPoints;
    [SerializeField] private float walkSpeedPercent;
    [SerializeField] private float turnAroundDelay;
    [SerializeField] private float patrolPointReachedDistance;

    private int currentPatrolPointIndex = 0;
    private float turnAroundTime = 0;
    public override void Do()
    {
        if (Time.time < turnAroundTime)
            movement.Move(0);
        else if (Math.Abs(patrolPoints[currentPatrolPointIndex].x - transform.position.x) < patrolPointReachedDistance)
        {
            movement.Move(0);
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            turnAroundTime = Time.time + turnAroundDelay;
        }
        else if (patrolPoints[currentPatrolPointIndex].x > transform.position.x)
            movement.Move(walkSpeedPercent);
        else
            movement.Move(-walkSpeedPercent);
    }
}
