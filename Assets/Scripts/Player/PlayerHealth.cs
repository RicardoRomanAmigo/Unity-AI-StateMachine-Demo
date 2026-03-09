using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    // Player health stats
    private float maxHealth = 100f;
    private float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnPlayerDied;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth - damage >= 0)
        {
            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth);
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
        // Handle player death (e.g., play animation, disable controls, etc.)
    }
}
