using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private CheckpointManager checkpointManager;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmmount)
    {
        currentHealth -= damageAmmount;
        if (damageAmmount > 0)
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
            if (deadBody)
            {
                var body = Instantiate(deadBody, transform.position, Quaternion.identity);
                body.transform.localScale = transform.localScale;
            }
            if (reloadCheckpointOnDeath)
            {
                checkpointManager.Reload();
            }
            Destroy(gameObject);
        }
        else if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

}
