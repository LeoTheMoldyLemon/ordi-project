using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private GameObject deadBody;
    private Animator animator;

    [SerializeField] private GameObject particleEffect;
    [SerializeField] private Transform particleEffectRoot;


    [SerializeField] private bool reloadCheckpointOnDeath = false;
    [SerializeField] private bool destroyOnDeath = false;
    [SerializeField] private bool resetHealthOnDeath = false;

    [SerializeField] private List<Func<Damage, bool>> checkInvincibilityFunctions;

    [SerializeField] private CheckpointManager checkpointManager;
    public UnityEvent death = new();

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
        foreach (var checkInvincibilityFunction in checkInvincibilityFunctions)
            if (checkInvincibilityFunction.Invoke(damage))
                return;

        if (Math.Sign(transform.localScale.x) == Math.Sign(transform.position.x - damage.transform.position.x) && damage.isBackstab)
            currentHealth -= damage.amount * 2;
        else
            currentHealth -= damage.amount;

        if (damage.amount > 0)
        {
            animator.SetTrigger("TakeDamage");
            if (particleEffect)
            {
                if (particleEffectRoot)
                    Instantiate(particleEffect, transform.position, particleEffect.transform.rotation, particleEffectRoot);
                else
                    Instantiate(particleEffect, transform.position, particleEffect.transform.rotation);
            }
        }

        if (currentHealth <= 0)
        {
            death.Invoke();
            if (deadBody)
            {
                var body = Instantiate(deadBody, transform.position, Quaternion.identity);
                body.transform.localScale = transform.localScale;
            }
            if (reloadCheckpointOnDeath)
                checkpointManager.Reload();

            if (destroyOnDeath)
                Destroy(gameObject);

            if (resetHealthOnDeath)
                currentHealth = maxHealth;
        }
        else if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

}
