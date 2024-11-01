using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Health>(out Health enemyHealth))
        {
            enemyHealth.takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
