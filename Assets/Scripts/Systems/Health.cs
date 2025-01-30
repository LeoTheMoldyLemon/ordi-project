using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth = 10;
    private Animator animator;
    private Movement movement;
    private new Collider2D collider;

    [SerializeField] private bool reloadCheckpointOnDeath = false;
    [SerializeField] private bool resetHealthOnDeath = false;
    [SerializeField] private float reviveTime = 0;

    [SerializeField] private List<Func<Damage, bool>> checkInvincibilityFunctions = new();
    public UnityEvent death = new(), revival = new();
    public UnityEvent<Damage> takeDamage = new();

    private Coroutine reviveCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (currentHealth == 0) LoadDead();
    }

    public void AddCheckInvincibilityFunctions(Func<Damage, bool> function)
    {
        checkInvincibilityFunctions.Add(function);
    }

    public void TakeDamage(Damage damage)
    {
        if (currentHealth == 0) return;
        foreach (var checkInvincibilityFunction in checkInvincibilityFunctions)
            if (checkInvincibilityFunction.Invoke(damage))
            {
                if (animator) animator.SetTrigger("DeflectHit");
                return;
            }

        Debug.Log("Taking damage " + damage.name, this);
        damage.DamageTaken(collider);
        if (Math.Sign(transform.localScale.x) == Math.Sign(transform.position.x - damage.transform.position.x) && damage.isBackstab)
        {
            Debug.Log("Backstab", this);
            currentHealth -= damage.amount * 2;
        }
        else
            currentHealth -= damage.amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (animator && damage.amount > 0) animator.SetTrigger("TakeDamage");


        takeDamage.Invoke(damage);
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        currentHealth = 0;
        death.Invoke();
        if (animator)
        {
            animator.SetBool("Dead", true);
            animator.SetTrigger("Dying");
        }
        if (movement) movement.Move(0);

        if (reloadCheckpointOnDeath)
            CheckpointManager.Instance.Reload();

        if (resetHealthOnDeath)
            currentHealth = maxHealth;
    }

    public void LoadDead()
    {
        currentHealth = 0; if (animator)
        {
            animator.SetBool("Dead", true);
            animator.SetTrigger("AlreadyDead");
        }
        death.Invoke();
    }

    public void Revive()
    {
        Debug.Log("Attempting revive of " + name);
        if (currentHealth != 0) return;

        Debug.Log("Reviving " + name); currentHealth = 0; if (animator)
        {
            animator.SetBool("Dead", false);
            animator.SetTrigger("Revive");
        }
        reviveCoroutine = StartCoroutine(ReviveCoroutine());
    }

    public void CancelRevive()
    {
        if (reviveCoroutine != null)
        {
            StopCoroutine(reviveCoroutine);
            Debug.Log("Reviving " + name); currentHealth = 0; if (animator)
            {
                animator.SetBool("Dead", true);
                animator.SetTrigger("Dying");
            }
        }
    }

    private IEnumerator ReviveCoroutine()
    {
        yield return new WaitForSeconds(reviveTime);
        currentHealth = maxHealth;
        revival.Invoke();
    }
}
