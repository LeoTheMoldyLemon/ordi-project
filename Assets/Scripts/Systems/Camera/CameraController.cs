using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour


{
    public new Camera camera;
    private Rigidbody2D targetRigidbody;
    public GameObject target;
    public float defaultSize = 6;
    public float smoothTime = 0.2f, smoothTimeSize = 0.2f, velocityScaling = 0.4f;
    private Vector3 smoothVelocity = Vector3.zero;
    private Vector3 targetPosition;


    public readonly SortedSet<CameraDock> docks = new(CameraDock.comparer);

    void Awake()
    {
        camera = GetComponent<Camera>();
        targetRigidbody = target.GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, -defaultSize / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));
        transform.position = targetPosition;
    }

    void Update()
    {
        CameraDock dock = docks.Max;
        if (dock == null)
        {
            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, -defaultSize / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));
            targetPosition += new Vector3(targetRigidbody.velocity.x * velocityScaling, 1, 0);
        }
        else
        {
            targetPosition = new Vector3(0, 0, 0);
            foreach (var dockTarget in dock.targets)
            {
                targetPosition += new Vector3(dockTarget.transform.position.x, dockTarget.transform.position.y, -dock.distance);
            }
            if (dock.lerp)
            {
                targetPosition += new Vector3(target.transform.position.x, target.transform.position.y, -dock.distance);
                targetPosition /= dock.targets.Length + 1;
            }
            else targetPosition /= dock.targets.Length;

        }


        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref smoothVelocity,
            smoothTime);

    }
    public void AddDock(CameraDock dock)
    {
        docks.Add(dock);
    }
    public void RemoveDock(CameraDock dock)
    {
        docks.Remove(dock);
    }

}
