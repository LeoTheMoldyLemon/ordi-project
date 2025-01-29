using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleStarter : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;
    public void PlayParticles(string name)
    {
        foreach (var particleSystem in particleSystems)
        {
            if (particleSystem.name == name)
            {
                particleSystem.Play();
            }
        }
    }
}
