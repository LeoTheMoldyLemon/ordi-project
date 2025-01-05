using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private GameObject deadBody;
    private Animator animator;

    [SerializeField] private GameObject particleEffect;
    [SerializeField] private Transform particleEffectRoot;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmmount)
    {
        currentHealth -= damageAmmount;

        animator.SetTrigger("TakeDamage");

        if (particleEffect)
        {
            if (particleEffectRoot)
                Instantiate(particleEffect, this.transform.position, particleEffect.transform.rotation, particleEffectRoot);
            else
                Instantiate(particleEffect, this.transform.position, particleEffect.transform.rotation);
        }

        if (currentHealth <= 0)
        {
            if (deadBody)
            {
                var body = Instantiate(deadBody, transform.position, Quaternion.identity);
                body.transform.localScale = transform.localScale;
            }
            Destroy(gameObject);
        }
    }
}
