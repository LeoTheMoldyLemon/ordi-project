using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject damageObjectPrefab;
    [SerializeField] private float windup = 0.1f;
    [SerializeField] private float cooldown = 0.5f;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private float speed = 0;
    [SerializeField] private bool isAimable;


    private bool hasAttackStarted = false;
    private float attackStartTimestamp = 0;
    private Vector2 attackDirection;


    void Update()
    {
        if (hasAttackStarted && attackStartTimestamp + windup <= Time.time)
        {
            var rotation = Quaternion.FromToRotation(Vector2.up, attackDirection);
            var attackObject = Instantiate(damageObjectPrefab, transform.position + positionOffset + rotation * damageObjectPrefab.transform.position, rotation);
            var attackRigidbody = attackObject.GetComponent<Rigidbody>();
            if (attackRigidbody)
                attackRigidbody.velocity = attackDirection.normalized * speed;
            hasAttackStarted = false;
        }
    }

    public bool Perform(Vector2 direction)
    {
        if (attackStartTimestamp + cooldown <= Time.time)
        {
            hasAttackStarted = true;
            attackStartTimestamp = Time.time;
            if (isAimable)
                attackDirection = direction;
            else
                attackDirection = new Vector2(direction.x, 0);
            return true;
        }
        return false;
    }
}
