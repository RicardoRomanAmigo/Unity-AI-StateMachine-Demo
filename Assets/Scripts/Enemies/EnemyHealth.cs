using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamagable
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
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            OnHealthChanged?.Invoke();
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        GetComponent<EnemyController>().TransitionToDead();
    }
}
