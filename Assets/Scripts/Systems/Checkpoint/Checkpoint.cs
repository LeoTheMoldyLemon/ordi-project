using System;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private CheckpointManager checkpointManager;
    private Animator animator;
    public bool Active { get; private set; } = false;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Active)
        {
            checkpointManager.Save(this);
        }
    }

    public void Activate()
    {
        Active = true;
        animator.SetBool("Active", true);
    }
    public void Deactivate()
    {
        Active = false;
        animator.SetBool("Active", false);
    }
}