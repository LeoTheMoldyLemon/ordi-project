using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject damageObjectPrefab;
    [SerializeField] private float windup = 0.1f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform originTransform;
    private Animator animator;

    public bool isAttacking = false;
    private float attackStartTimestamp = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isAttacking && attackStartTimestamp + windup <= Time.time)
        {
            var attackObject = Instantiate(damageObjectPrefab, originTransform.position, originTransform.rotation);
            var attackRigidbody = attackObject.GetComponent<Rigidbody>();
            if (attackRigidbody)
                attackRigidbody.velocity = originTransform.forward * speed;
            isAttacking = false;
        }
    }

    public bool Perform()
    {
        if (attackStartTimestamp + cooldown + windup <= Time.time)
        {
            if (animator) animator.SetTrigger("MakeAttack");
            isAttacking = true;
            attackStartTimestamp = Time.time;
            return true;
        }
        return false;
    }
}
