using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float baseMovementSpeed = 1;
    [SerializeField] private float baseJumpSpeed = 10;
    [SerializeField] private float baseMovementSmoothTime = 0.1f;
    [SerializeField] private float aerialMovementSmoothTime = 1f;
    [SerializeField] private float movementSpeedModifier = 1;

    private float targetDirection = 0;
    private float currentMovementDirectionModifier = 0;
    private float smoothMovementVelocity = 0;
    public bool IsJumping { get; private set; }
    public bool IsGrounded { get; private set; }


    private new Rigidbody2D rigidbody;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        currentMovementDirectionModifier = Mathf.SmoothDamp(
            currentMovementDirectionModifier,
            targetDirection,
            ref smoothMovementVelocity,
            IsGrounded ? baseMovementSmoothTime : aerialMovementSmoothTime);
    }

    public void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(baseMovementSpeed * movementSpeedModifier * currentMovementDirectionModifier, rigidbody.velocity.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            IsJumping = false;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    public void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, baseJumpSpeed);
        IsJumping = true;
    }
    public void StopJump()
    {
        if (rigidbody.velocity.y > 0 && IsJumping)
        {
            rigidbody.velocity = Vector2.Scale(rigidbody.velocity, new Vector2(1, 0.3f));
            IsJumping = false;
        }
    }

    public void Move(float direction)
    {
        targetDirection = direction;
    }

    public void AddMovementSpeedModifier(float modifier)
    {
        movementSpeedModifier *= modifier;
    }

    public float GetMovementDirection()
    {
        return targetDirection;
    }

}
