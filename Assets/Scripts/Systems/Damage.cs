using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    public int amount;
    public bool isBackstab;
    public DamageType type;
    [SerializeField] private bool isSingleFrame;

    enum OnHitAction
    {
        DISAPPEAR,
        STICK,
        PIERCE
    }
    public enum DamageType
    {
        RANGED,
        MELEE,
        ENVIRONMENTAL
    }


    [SerializeField] private OnHitAction onHitTarget;
    [SerializeField] private OnHitAction onHitStructure;

    public UnityEvent<Damage, Collider2D> hitEvent = new();
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (isSingleFrame)
        {
            var collider = GetComponent<Collider2D>();
            List<Collider2D> collisions = new();
            var contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(collider.includeLayers);
            contactFilter.useLayerMask = true;
            Physics2D.OverlapCollider(collider, contactFilter, collisions);

            foreach (Collider2D collision in collisions)
                Hit(collision);

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
        Debug.Log("hit " + collision.name, this);
        if (collision.gameObject.CompareTag("Structure"))
        {
            if (animator != null) animator.SetTrigger("HitStructure");
            hitEvent.Invoke(this, collision);
            TakeAction(onHitStructure, collision);
        }
        else if (collision.gameObject.TryGetComponent(out Health targetHealth))
        {
            if (animator != null) animator.SetTrigger("HitTarget");
            targetHealth.TakeDamage(this);
        }
    }

    public void DamageTaken(Collider2D collision)
    {
        hitEvent.Invoke(this, collision);
        TakeAction(onHitTarget, collision);
    }

    private void TakeAction(OnHitAction actionToTake, Collider2D collision)
    {
        switch (actionToTake)
        {
            case OnHitAction.DISAPPEAR:
                Destroy(gameObject);
                break;
            case OnHitAction.STICK:
                //transform.parent = collision.transform;
                var rigidbody = GetComponent<Rigidbody2D>();
                if (rigidbody)
                {
                    rigidbody.gravityScale = 0;
                    rigidbody.velocity = Vector2.zero;
                    rigidbody.freezeRotation = true;
                    Destroy(this);
                }
                break;
            case OnHitAction.PIERCE:
                break;
        }
    }
}


