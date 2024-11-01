using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Movement movement;

    [SerializeField] private InputActionReference movementInput;
    void Start()
    {
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.Move(movementInput.action.ReadValue<Vector2>().x);

        if (movementInput.action.ReadValue<Vector2>().y > 0)
        {
            if (movement.IsGrounded())
                movement.Jump();
        }
        else if (movement.IsJumping())
            movement.StopJump();

    }
}
