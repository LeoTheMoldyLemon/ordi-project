using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableHealth
{
    public int currentHealth;

    public SerializableHealth(Health health)
    {
        currentHealth = health.currentHealth;
    }

    public void Update(Health health)
    {
        health.currentHealth = currentHealth;
        if (health.currentHealth <= 0)
            health.LoadDead();

    }
}