using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour


{
    public new Camera camera;
    private Rigidbody2D targetRigidbody;
    public GameObject target, defaultTarget;
    public float targetSize = 6, defaultSize = 6;
    public float smoothTime = 0.2f, smoothTimeSize = 0.2f, velocityScaling = 0.4f;
    private Vector3 smoothVelocity = Vector3.zero;
    private Vector3 targetPosition;
    private float smoothVelocitySize = 0f, currentAspect;


    private readonly SortedSet<CameraDock> docks = new(CameraDock.comparer);

    void Awake()
    {
        camera = GetComponent<Camera>();
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        currentAspect = camera.aspect;
    }

    public void Start()
    {
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, -GetDistanceForSize(targetSize));
        transform.position = targetPosition;
    }

    public void SetTarget(GameObject target, float size)
    {
        this.target = target;
        targetSize = size;
    }
    public void SetDefaultTarget()
    {
        target = defaultTarget;
        targetSize = defaultSize;
    }

    void Update()
    {
        if (currentAspect != camera.aspect)
            UpdateCameraTarget();
        if (!target) return;
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, -GetDistanceForSize(targetSize));
        if (target == defaultTarget) targetPosition += new Vector3(targetRigidbody.velocity.x * velocityScaling, 1, 0);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref smoothVelocity,
            smoothTime);

        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, targetSize, ref smoothVelocitySize, smoothTimeSize);

    }

    private void UpdateCameraTarget()
    {
        CameraDock dock = docks.Max;
        if (dock != null)
        {
            float minHeightSize = dock.minHeight / 2f;
            float minWidthSize = dock.minWidth / (2f * camera.aspect);
            SetTarget(dock.gameObject, Math.Max(minWidthSize, minHeightSize));
        }
        else
        {
            SetDefaultTarget();
        }
    }
    public void AddDock(CameraDock dock)
    {
        docks.Add(dock);
        UpdateCameraTarget();
    }
    public void RemoveDock(CameraDock dock)
    {
        docks.Remove(dock);
        UpdateCameraTarget();
    }

    private float GetDistanceForSize(float size)
    {
        return size / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }
}
