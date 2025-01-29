using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDock : MonoBehaviour
{
    public float minWidth, minHeight, distance;
    public bool lerp = false;
    public float marginSize = 1;
    public int priority;
    private CameraController cameraController;
    private GameObject player;
    public GameObject[] targets;

    public static IComparer<CameraDock> comparer = Comparer<CameraDock>.Create(
        (dock1, dock2) => dock1.priority - dock2.priority != 0 ? dock1.priority - dock2.priority : dock1.GetHashCode() - dock2.GetHashCode());


    void Awake()
    {
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
    }
    void Start()
    {
        player = PlayerController.Instance.gameObject;
        distance = GetDistance();
        if (targets.Length == 0) targets = new GameObject[] { gameObject };
    }

    private float GetDistance()
    {
        float height = minHeight, width = minWidth;
        if (lerp)
        {
            foreach (var target in targets)
            {
                height = Math.Max(height, Math.Abs(player.transform.position.y - target.transform.position.y) + marginSize * 2);
                width = Math.Max(width, Math.Abs(player.transform.position.x - target.transform.position.x) + marginSize * 2);
            }
        }
        float heightSize = height / 2f;
        float widthSize = width / (2f * Camera.main.aspect);
        float size = Math.Max(widthSize, heightSize);
        return size / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }
    void Update()
    {
        if (lerp && this == cameraController.docks.Max) distance = GetDistance();
    }

    public void OnDrawGizmos()
    {
        Camera camera = Camera.main;
        float minHeightSize = minHeight / 2f;
        float minWidthSize = minWidth / (2f * camera.aspect);
        float height = 2f * Math.Max(minWidthSize, minHeightSize);
        float width = height * camera.aspect;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(minWidth, minHeight, 0));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") Activate();
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") Deactivate();
    }

    public void Activate()
    {
        cameraController.AddDock(this);

    }
    public void Deactivate()
    {
        cameraController.RemoveDock(this);

    }

}
