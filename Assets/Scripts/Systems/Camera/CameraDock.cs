using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDock : MonoBehaviour
{
    public float minWidth;
    public float minHeight;
    public int priority;
    private CameraController cameraController;

    public static IComparer<CameraDock> comparer = Comparer<CameraDock>.Create(
        (dock1, dock2) => dock1.priority - dock2.priority != 0 ? dock1.priority - dock2.priority : dock1.GetHashCode() - dock2.GetHashCode());


    void Awake()
    {
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
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
        if (other.gameObject.tag == "Player")
            cameraController.AddDock(this);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            cameraController.RemoveDock(this);
    }

}
