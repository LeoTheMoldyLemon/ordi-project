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
    public bool isOnCooldown = false;
    private float attackStartTimestamp = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Update()
    {
        if (isOnCooldown && attackStartTimestamp + cooldown + windup <= Time.time)
        {
            isOnCooldown = false;
            animator.SetBool("AttackCooldown", false);
        }
        if (isAttacking && attackStartTimestamp + windup <= Time.time)
        {
            var attackObject = Instantiate(damageObjectPrefab, originTransform.position + Vector3.Scale(damageObjectPrefab.transform.position, this.transform.localScale), damageObjectPrefab.transform.rotation);
            var attackRigidbody = attackObject.GetComponent<Rigidbody>();
            if (attackRigidbody)
                attackRigidbody.velocity = originTransform.forward * speed;
            isAttacking = false;
            isOnCooldown = true;
            animator.SetBool("AttackWindup", false);
            animator.SetBool("AttackCooldown", true);
        }
    }

    public bool Perform()
    {
        if (!isAttacking && !isOnCooldown)
        {
            Debug.Log(this.name + ": Performing attack");
            if (animator)
            {
                animator.SetBool("AttackWindup", true);
                animator.SetTrigger("MakeAttack");
            }
            isAttacking = true;
            attackStartTimestamp = Time.time;
            return true;
        }
        return false;
    }
}
