using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Moving")]
    [SerializeField] private float baseMovementSpeed = 1;
    [SerializeField] private float aerialMovementSmoothTime = 1f;
    [SerializeField] private float movementSpeedModifier = 1;
    [SerializeField] private float baseMovementSmoothTime = 0.1f;


    [Header("Jumping")]
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float jumpWarmupTime = 2 / 12;
    [SerializeField] private float coyoteTime = 2 / 12;


    [Header("Wall jumping")]
    [SerializeField] private bool canWallJump = false;
    [SerializeField] private float wallJumpSpeed = 10;
    [SerializeField] private float wallJumpPushoffSpeed = 5;
    [SerializeField] private float wallJumpWarmupTime = 1 / 12;
    [SerializeField] private float wallSlideMaxSpeed = 0.5f;

    [Header("Rolling")]
    [SerializeField] private bool canRoll = false;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float rollDuration = 0.3f;
    [SerializeField] private float rollCooldownTime = 0.5f;
    [SerializeField] private float rollWarmupTime = 0.1f;


    private float jumpStartTime = 0, rollStartTime = 0;
    private float targetDirection = 0;
    private float smoothMovementVelocity = 0;

    [Header("Debug Info")]
    public bool isJumping = false;
    public bool isHoldingWall = false;
    public bool isGrounded = false, isDropping = false;
    public bool isInGroundCoyoteTime = false, isWarmupJumping = false;
    public bool isInWallCoyoteTime = false, isWarmupWallJumping = false;
    public bool isRolling = false, isWarmupRolling = false, isCooldownRolling = false;
    public float currentMovementDirectionModifier = 0;

    private float groundCoyoteTimestamp = 0f, wallCoyoteTimestamp = 0f;


    public Vector2 facing = new(-1, 0);

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Animator animator;
    private Health health;

    public readonly HashSet<GameObject> ignoredPlatforms = new();
    private readonly HashSet<Collider2D> collidedPlatforms = new();

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    public void Start()
    {
        isGrounded = CheckIsGrounded();
        if (health != null)
            health.AddCheckInvincibilityFunctions(CheckIsInvincible);
        else
            Debug.LogWarning("Health module not found on " + name);
    }

    public void OnDrawGizmos()
    {

        Collider2D collider = GetComponent<Collider2D>();

        //CheckIsGrounded boxcast bounds
        Vector3 boxcastBounds = new(collider.bounds.extents.x * 2, collider.bounds.extents.y, collider.bounds.extents.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, boxcastBounds.y / 2 + 0.1f, 0), boxcastBounds);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, boxcastBounds.y / 2, 0), boxcastBounds);

        //CheckIsHoldingWall boxcast bounds
        boxcastBounds = new(collider.bounds.extents.x, collider.bounds.extents.y * 1.8f, collider.bounds.extents.z * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(boxcastBounds.x / 2 + 0.1f, 0, 0) * Math.Sign(targetDirection), boxcastBounds);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3(boxcastBounds.x / 2, 0, 0) * Math.Sign(targetDirection), boxcastBounds);


    }

    private bool CheckIsGrounded()
    {
        Vector3 boxcastBounds = new(collider.bounds.extents.x * 2, collider.bounds.extents.y, collider.bounds.extents.z * 2);

        return rigidbody.velocity.y == 0.0 &&
            Physics2D.BoxCast(
                transform.position - new Vector3(0, boxcastBounds.y / 2, 0),
                boxcastBounds,
                0,
                Vector3.down,
                0.1f,
                LayerMask.GetMask("Structure", "Platform"));
    }

    private bool CheckIsInvincible(Damage damage)
    {
        return isRolling && damage.type != Damage.DamageType.ENVIRONMENTAL;
    }

    private bool CheckIsHoldingWall()
    {
        if (rigidbody.velocity.y > 10 || isGrounded || !canWallJump) return false;

        Vector3 boxcastBounds = new(collider.bounds.extents.x, collider.bounds.extents.y * 1.8f, collider.bounds.extents.z * 2);

        return Physics2D.BoxCast(
                transform.position + new Vector3(boxcastBounds.x / 2, 0, 0) * Math.Sign(facing.x),
                boxcastBounds,
                0,
                new Vector2(facing.x, 0),
                0.1f,
                LayerMask.GetMask("Structure"));
    }

    public void FixedUpdate()
    {
        if (isInGroundCoyoteTime && groundCoyoteTimestamp + coyoteTime < Time.time)
            isInGroundCoyoteTime = false;
        if (isInWallCoyoteTime && wallCoyoteTimestamp + coyoteTime < Time.time)
            isInWallCoyoteTime = false;

        if (!isRolling)
            currentMovementDirectionModifier = Mathf.SmoothDamp(
                rigidbody.velocity.x / (baseMovementSpeed * movementSpeedModifier),
                targetDirection,
                ref smoothMovementVelocity,
                isGrounded ? baseMovementSmoothTime : aerialMovementSmoothTime);

        if (Math.Abs(currentMovementDirectionModifier) < 0.005f && targetDirection == 0) currentMovementDirectionModifier = 0;

        float newVelocityY = rigidbody.velocity.y;
        float newVelocityX = baseMovementSpeed * movementSpeedModifier * currentMovementDirectionModifier;

        if (isWarmupJumping && Time.time > jumpStartTime + jumpWarmupTime)
        {
            newVelocityY = jumpSpeed;
            isJumping = true;
            isWarmupJumping = false;
        }
        if (isWarmupWallJumping && Time.time > jumpStartTime + wallJumpWarmupTime)
        {
            newVelocityY = wallJumpSpeed;
            newVelocityX = wallJumpPushoffSpeed * Math.Sign(facing.x);
            currentMovementDirectionModifier = newVelocityX / (baseMovementSpeed * movementSpeedModifier);
            isJumping = true;
            isWarmupWallJumping = false;
        }
        if (isWarmupRolling && Time.time > rollStartTime + rollWarmupTime)
        {
            newVelocityX = rollSpeed * Math.Sign(facing.x);
            currentMovementDirectionModifier = newVelocityX / (baseMovementSpeed * movementSpeedModifier);
            isRolling = true;
            isWarmupRolling = false;
        }
        else if (isRolling && Time.time > rollStartTime + rollWarmupTime + rollDuration)
        {
            animator.SetBool("Rolling", false);
            isRolling = false;
            isCooldownRolling = true;
        }
        else if (isCooldownRolling && Time.time > rollStartTime + rollWarmupTime + rollDuration + rollCooldownTime)
        {
            isCooldownRolling = false;
        }


        if (Math.Abs(rigidbody.velocity.x) < 0.01 && CheckIsHoldingWall())
        {
            if (!isHoldingWall)
            {
                isHoldingWall = true;
                isJumping = false;
                animator.SetBool("HoldingWall", true);
            }
        }
        else
        {
            if (isHoldingWall)
            {
                isInWallCoyoteTime = true;
                wallCoyoteTimestamp = Time.time;
                isHoldingWall = false;
                animator.SetBool("HoldingWall", false);
            }
        }

        if (isHoldingWall && newVelocityY < -wallSlideMaxSpeed)
            newVelocityY = -wallSlideMaxSpeed;

        animator.SetFloat("VelocityX", rigidbody.velocity.x);
        animator.SetFloat("VelocityY", rigidbody.velocity.y);

        if (targetDirection != 0 && Math.Abs(rigidbody.velocity.x) > 1)
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);


        if (Math.Abs(newVelocityX) < 0.05f) newVelocityX = 0;

        rigidbody.velocity = new Vector2(newVelocityX, newVelocityY);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            if (CheckIsGrounded())
            {
                isGrounded = true;
                animator.SetBool("IsGrounded", true);
                isJumping = false;
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                if (isDropping)
                {
                    Physics2D.IgnoreCollision(collider, collision.collider, true);
                    ignoredPlatforms.Add(collision.gameObject);
                }
                collidedPlatforms.Add(collision.collider);
            }
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Structure"))
        {
            if (!CheckIsGrounded() && isGrounded)
            {
                if (!isJumping && !isWarmupJumping)
                {
                    Debug.Log("CoyoteTimeActive");
                    isInGroundCoyoteTime = true;
                    groundCoyoteTimestamp = Time.time;
                }
                isGrounded = false;
                animator.SetBool("IsGrounded", false);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
                collidedPlatforms.Remove(collision.collider);

        }
    }

    public void Jump()
    {
        if (!isWarmupJumping && !isJumping && !isWarmupWallJumping && !isRolling && !isWarmupRolling)
        {
            if (isGrounded || isInGroundCoyoteTime)
            {
                Debug.Log("Jump");
                Debug.Log(isGrounded);
                Debug.Log(isInGroundCoyoteTime);
                isWarmupJumping = true;
                isInGroundCoyoteTime = false;
                jumpStartTime = Time.time;
                animator.SetTrigger("Jump");
            }
            if (isHoldingWall || isInWallCoyoteTime)
            {
                Debug.Log("Walljump");
                Debug.Log(isHoldingWall);
                Debug.Log(isInWallCoyoteTime);
                isWarmupWallJumping = true;
                isInWallCoyoteTime = false;
                jumpStartTime = Time.time;
                facing = -facing;
                animator.SetTrigger("WallJump");
            }
        }
    }

    public void StopJump()
    {
        if (rigidbody.velocity.y > 0 && isJumping)
        {
            rigidbody.velocity = Vector2.Scale(rigidbody.velocity, new Vector2(1, 0.3f));
            isJumping = false;
        }
    }

    public void Roll()
    {
        if (isGrounded && !isJumping && !isWarmupJumping && !isWarmupRolling && !isRolling && !isCooldownRolling && canRoll)
        {
            isWarmupRolling = true;
            rollStartTime = Time.time;
            animator.SetTrigger("Roll");
            animator.SetBool("Rolling", true);
        }
    }

    public void Drop()
    {
        isDropping = true;

        foreach (Collider2D platformCollider in collidedPlatforms)
        {
            ignoredPlatforms.Add(platformCollider.gameObject);
            Physics2D.IgnoreCollision(collider, platformCollider, true);
        }
    }

    public void StopDrop()
    {
        isDropping = false;
    }

    public void Move(float direction)
    {
        if (targetDirection != direction)
        {
            targetDirection = direction;
        }
        if (targetDirection != 0 && !isHoldingWall && !isInWallCoyoteTime && facing.x != Math.Sign(targetDirection))
            facing = new Vector2(Math.Sign(targetDirection), 0);

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
