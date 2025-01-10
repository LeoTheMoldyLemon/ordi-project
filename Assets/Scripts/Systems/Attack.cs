using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject damageObjectPrefab;
    [SerializeField] private float windup = 0.1f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform originTransform;
    [SerializeField] private bool waitUntilAttackHits, stickWithParent;
    [SerializeField] private Animator animator;

    public bool isOnWindup = false;
    public bool isAttacking = false;
    public bool isOnCooldown = false;
    public string attackName = "";
    private float attackCooldownTimestamp = 0;


    public void Update()
    {

        if (isOnCooldown && Time.time > attackCooldownTimestamp + cooldown)
        {
            isOnCooldown = false;
            animator.SetBool(attackName + "AttackCooldown", false);
        }
    }

    public bool Perform(params UnityAction<Damage, Collider2D>[] onHitActions)
    {
        if (!isOnWindup && !isOnCooldown && !isAttacking)
        {
            if (animator)
            {
                animator.SetBool(attackName + "AttackWindup", true);
                animator.SetTrigger(attackName + "MakeAttack");
            }
            isOnWindup = true;
            StartCoroutine(Process(onHitActions));
            return true;
        }
        return false;
    }

    private IEnumerator Process(UnityAction<Damage, Collider2D>[] onHitActions)
    {
        yield return new WaitForSeconds(windup);

        GameObject damageObject;
        if (stickWithParent)
            damageObject = Instantiate(damageObjectPrefab, originTransform);
        else
            damageObject = Instantiate(damageObjectPrefab, originTransform.position + Vector3.Scale(damageObjectPrefab.transform.position, originTransform.localScale), damageObjectPrefab.transform.rotation);

        var damageRigidbody = damageObject.GetComponent<Rigidbody>();
        if (damageRigidbody)
            damageRigidbody.velocity = originTransform.forward * speed;
        var damage = damageObject.GetComponent<Damage>();
        if (damage)
        {
            foreach (var onHitAction in onHitActions)
                damage.hitEvent.AddListener(onHitAction);
            if (waitUntilAttackHits)
                damage.hitEvent.AddListener(OnHit);
        }

        if (waitUntilAttackHits)
        {
            isAttacking = true;
            animator.SetBool(attackName + "Attacking", true);
        }
        else
        {
            isOnCooldown = true;
            attackCooldownTimestamp = Time.time;
            animator.SetBool(attackName + "AttackCooldown", true);
        }

        isOnWindup = false;
        animator.SetBool(attackName + "AttackWindup", false);
    }

    private void OnHit(Damage damage, Collider2D collider)
    {
        isOnCooldown = true;
        attackCooldownTimestamp = Time.time;
        animator.SetBool(attackName + "AttackCooldown", true);

        isAttacking = false;
        animator.SetBool(attackName + "Attacking", false);
    }
}
