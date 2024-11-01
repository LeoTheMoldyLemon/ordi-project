using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Movement movement;

    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private Attack primaryAttack;
    [SerializeField] private Vector2 facing;
    void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        movement.Move(movementInput.action.ReadValue<Vector2>().x);

        if (movementInput.action.ReadValue<Vector2>().y > 0)
        {
            if (movement.IsGrounded)
                movement.Jump();
        }
        else if (movement.IsJumping)
            movement.StopJump();

        if (attackInput.action.ReadValue<float>() > 0)
        {
            primaryAttack.Perform(facing);
        }

    }
}
