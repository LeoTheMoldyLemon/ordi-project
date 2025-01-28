using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MagicAttackDamageBehaviour : MonoBehaviour
{
    public float speed, rotationSpeed, aimingTime;
    private float startTimestamp;
    public Transform target;
    private new Rigidbody2D rigidbody;
    private bool launched = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startTimestamp = Time.time;
        if (!target) target = PlayerController.Instance.transform;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (!launched)
        {
            if (Time.time > startTimestamp + aimingTime)
            {
                launched = true;
                rigidbody.velocity = transform.right * transform.localScale.x * speed;
            }
            float angle = Vector2.SignedAngle(transform.right * transform.localScale.x, target.position - transform.position);
            if (Math.Abs(angle) < rotationSpeed * Time.deltaTime)
                rigidbody.rotation += angle;
            else
                rigidbody.rotation += angle * rotationSpeed * Time.deltaTime;
        }
    }
}
