using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Movement movement;
    private new Rigidbody2D rigidbody;
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private Attack primaryAttack;
    void Awake()
    {
        movement = GetComponent<Movement>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movementInputVector = movementInput.action.ReadValue<Vector2>();
        movement.Move(movementInputVector.x);

        if (movementInputVector.y > 0)
        {
            if (movement.isGrounded)
                movement.Jump();
        }
        else if (movement.IsJumping)
            movement.StopJump();

        if (attackInput.action.ReadValue<float>() > 0)
            primaryAttack.Perform();

    }

}
