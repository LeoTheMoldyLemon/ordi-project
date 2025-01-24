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
    [SerializeField] private GameObject deadBody;
    private Animator animator;

    [SerializeField] private GameObject particleEffect;
    [SerializeField] private Transform particleEffectRoot;


    [SerializeField] private bool reloadCheckpointOnDeath = false;
    [SerializeField] private bool destroyOnDeath = false;
    [SerializeField] private bool resetHealthOnDeath = false;

    [SerializeField] private List<Func<Damage, bool>> checkInvincibilityFunctions = new();

    [SerializeField] private CheckpointManager checkpointManager;
    public UnityEvent death = new();
    public UnityEvent<Damage> takeDamage = new();

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void AddCheckInvincibilityFunctions(Func<Damage, bool> function)
    {
        checkInvincibilityFunctions.Add(function);
    }

    public void TakeDamage(Damage damage)
    {
        Debug.Log("Taking damage " + damage.name, this);
        foreach (var checkInvincibilityFunction in checkInvincibilityFunctions)
            if (checkInvincibilityFunction.Invoke(damage))
            {
                Debug.Log("Nope. " + damage.name, this);
                return;
            }

        if (Math.Sign(transform.localScale.x) == Math.Sign(transform.position.x - damage.transform.position.x) && damage.isBackstab)
        {
            Debug.Log("Backstab", this);
            currentHealth -= damage.amount * 2;
        }
        else
            currentHealth -= damage.amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (damage.amount > 0)
        {
            if (animator)
                animator.SetTrigger("TakeDamage");
            if (particleEffect)
            {
                if (particleEffectRoot)
                    Instantiate(particleEffect, transform.position, particleEffect.transform.rotation, particleEffectRoot);
                else
                    Instantiate(particleEffect, transform.position, particleEffect.transform.rotation);
            }
        }

        takeDamage.Invoke(damage);
        if (currentHealth <= 0)
            Die();
    }

    public void Die(bool skipParticles = false)
    {
        currentHealth = 0;
        death.Invoke();
        if (deadBody)
        {
            var body = Instantiate(deadBody, transform.position, Quaternion.identity);
            if (skipParticles)
                foreach (var particleSystem in body.GetComponentsInChildren<ParticleSystem>())
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            body.transform.localScale = transform.localScale;
        }
        if (reloadCheckpointOnDeath)
            checkpointManager.Reload();

        if (destroyOnDeath)
            Destroy(gameObject);

        if (resetHealthOnDeath)
            currentHealth = maxHealth;
    }

}
