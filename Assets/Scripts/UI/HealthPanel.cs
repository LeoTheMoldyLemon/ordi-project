using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPanel : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite heart, damagedHeart;
    [SerializeField] private Health health;
    void Start()
    {
        health.takeDamage.AddListener(UpdateUI);
    }

    void UpdateUI(Damage damage)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health.currentHealth) hearts[i].sprite = heart;
            else hearts[i].sprite = damagedHeart;
        }
    }
}
