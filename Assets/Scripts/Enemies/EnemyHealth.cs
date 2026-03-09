using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    private float currentHealth;

    public float CurrentHealth => currentHealth;

    public event Action OnHealthChanged;

    public void Initialize(float health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke();
    }


}
