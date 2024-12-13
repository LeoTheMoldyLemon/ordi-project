using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour


{
    private Camera camera;
    private Rigidbody2D targetRigidbody;
    [SerializeField] private GameObject target;
    private Vector3 smoothVelocity = Vector3.zero;

    void Start()
    {
        camera = GetComponent<Camera>();
        targetRigidbody = target.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.transform.position + new Vector3(targetRigidbody.velocity.x * 0.4f, 1, 0),
            ref smoothVelocity,
            0.2f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

    }
}
