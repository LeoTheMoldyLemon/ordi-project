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
    public Vector2 facing = new(-1, 0);
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
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
        if (collision.gameObject.CompareTag("Structure") && CheckIsGrounded())
        {
            IsGrounded = true;
            IsJumping = false;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Structure") && !CheckIsGrounded())
            IsGrounded = false;
    }

    private bool CheckIsGrounded()
    {
        return rigidbody.velocity.y == 0.0 &&
            Physics2D.BoxCast(transform.position, collider.bounds.extents, 0, -Vector3.up, collider.bounds.extents.y + 0.1f, LayerMask.GetMask("Structure"));
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
        if (targetDirection != 0)
            facing = new Vector2(targetDirection, 0);

        if (facing.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (facing.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

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
