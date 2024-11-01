using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatNPCController : MonoBehaviour
{
    private Movement movement;
    [SerializeField] private Attack primaryAttack;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    public void Move(float direction)
    {
        movement.Move(direction);
    }

    public void Jump()
    {
        movement.Jump();
    }

    public void Attack(GameObject target)
    {
        primaryAttack.Perform(target.transform.position - transform.position);
    }
}
