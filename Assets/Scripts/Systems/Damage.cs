using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.UIElements;
using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] private int damage;
    [SerializeField] private bool isSingleFrame;

    enum OnHitAction
    {
        DISAPEAR,
        STICK,
        PIERCE
    }

    [SerializeField] private OnHitAction onHitTarget;
    [SerializeField] private OnHitAction onHitStructure;
    [SerializeField] private string targetTag;

    void Start()
    {
        if (isSingleFrame)
        {
            var collider = GetComponent<Collider2D>();
            List<Collider2D> collisions = new();
            Physics2D.OverlapCollider(collider, new ContactFilter2D(), collisions);

            foreach (Collider2D collision in collisions)
            {
                Hit(collision);
            }
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSingleFrame)
        {
            Hit(collision);
        }
    }

    private void Hit(Collider2D collision)
    {
        var actionToTake = onHitStructure;

        if (collision.gameObject.TryGetComponent(out Health targetHealth) && collision.gameObject.CompareTag(targetTag))
        {
            targetHealth.TakeDamage(damage);
            actionToTake = onHitTarget;
        }

        switch (actionToTake)
        {
            case OnHitAction.DISAPEAR:
                Destroy(gameObject);
                break;
            case OnHitAction.STICK:
                transform.parent = collision.transform;
                var attackRigidbody = GetComponent<Rigidbody>();
                if (attackRigidbody)
                {
                    attackRigidbody.velocity = Vector2.zero;
                    attackRigidbody.freezeRotation = true;
                    Destroy(this);
                }
                break;
            case OnHitAction.PIERCE:
                break;
        }
    }
}


