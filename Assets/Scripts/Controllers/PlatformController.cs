using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private new Collider2D collider;
    void Awake()
    {
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            if (collider.usedByEffector) this.collider = collider;
            break;
        }
        if (!collider)
            throw new Exception("Missing Collider2D that is an effector.");

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponent<Movement>();
        if (movement)
        {
            movement.ignoredPlatforms.Remove(gameObject);
            Physics2D.IgnoreCollision(collider, other, false);
        }
    }
}
