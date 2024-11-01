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
    private bool isJumping;
    private bool isGrounded;


    private new Rigidbody2D rigidbody;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        currentMovementDirectionModifier = Mathf.SmoothDamp(currentMovementDirectionModifier, targetDirection, ref smoothMovementVelocity, isGrounded ? baseMovementSmoothTime : aerialMovementSmoothTime);
    }

    public void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(baseMovementSpeed * movementSpeedModifier * currentMovementDirectionModifier, rigidbody.velocity.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, baseJumpSpeed);
        isJumping = true;
    }
    public void StopJump()
    {
        if (rigidbody.velocity.y > 0 && isJumping)
        {
            rigidbody.velocity = Vector2.Scale(rigidbody.velocity, new Vector2(1, 0.3f));
            isJumping = false;
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

    public bool IsJumping()
    {
        return isJumping;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetMovementDirection()
    {
        return targetDirection;
    }

}
