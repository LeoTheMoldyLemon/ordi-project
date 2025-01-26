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
    [SerializeField] private InputActionReference movementInput, attackInput, rollInput;
    public PlayerInput playerInput;

    public static PlayerController Instance;
    [SerializeField] private Attack primaryAttack;
    void Awake()
    {
        Instance = this;
        movement = GetComponent<Movement>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        LoadUserRebinds();
    }


    void LoadUserRebinds()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        playerInput.actions.LoadBindingOverridesFromJson(rebinds);
    }

    void Update()
    {
        Vector2 movementInputVector = movementInput.action.ReadValue<Vector2>();
        float movementModifier = 1;
        if (primaryAttack.isOnWindup) movementModifier = 0.1f;

        movement.Move(movementInputVector.x * movementModifier);

        if (movementInputVector.y > 0 && !primaryAttack.isOnWindup && !primaryAttack.isAttacking)
        {
            movement.Jump();
        }
        else if (movement.isJumping)
            movement.StopJump();

        if (movementInputVector.y < 0 && !movement.isDropping)
            movement.Drop();
        else if (movementInputVector.y >= 0 && movement.isDropping)
            movement.StopDrop();


        if (attackInput.action.ReadValue<float>() > 0 && movement.isGrounded)
            primaryAttack.Perform();

        if (rollInput.action.ReadValue<float>() > 0)
            movement.Roll();

    }

}
