using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] public bool autoClose = false, isOpen = false, openOnStart = false;
    [SerializeField] private float autoCloseTime = 0;
    [SerializeField] private CameraDock cameraDock;
    [SerializeField] private float panTime;


    private Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        if (health)
            health.death.AddListener(() =>
            {
                if (cameraDock) StartCoroutine(PanCamera());
                OpenDoor();
            });
        if (openOnStart) OpenDoor();

    }

    public void OpenDoor()
    {
        if (isOpen) return;
        Debug.Log("Opening door");
        isOpen = true;
        animator.SetTrigger("OpenDoor");
        if (autoClose)
            Invoke(nameof(CloseDoor), autoCloseTime);
    }

    private IEnumerator PanCamera()
    {
        cameraDock.Activate();
        yield return new WaitForSeconds(panTime);
        cameraDock.Deactivate();
    }
    public void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;
        animator.SetTrigger("CloseDoor");
    }
}
