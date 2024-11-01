using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private GameObject deadBody;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmmount)
    {
        currentHealth -= damageAmmount;

        if (currentHealth <= 0)
        {
            if (deadBody)
                Instantiate(deadBody, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
