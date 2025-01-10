using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private bool autoClose = false;
    [SerializeField] private float openDuration = 0;


    private Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        health.death.AddListener(OpenDoor);
    }

    public void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        if (autoClose)
            Invoke(nameof(CloseDoor), openDuration);
    }
    public void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
    }
}
