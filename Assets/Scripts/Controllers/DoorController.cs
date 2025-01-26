using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] public bool autoClose = false, isOpen = false;
    [SerializeField] private float autoCloseTime = 0;


    private Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        health.death.AddListener(OpenDoor);
    }

    public void OpenDoor()
    {
        Debug.Log("openning door");
        if (isOpen) return;
        isOpen = true;
        animator.SetTrigger("OpenDoor");
        if (autoClose)
            Invoke(nameof(CloseDoor), autoCloseTime);
    }
    public void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;
        animator.SetTrigger("CloseDoor");
    }
}
