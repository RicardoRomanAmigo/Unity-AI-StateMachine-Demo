using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    // Player health stats
    public float MaxHealth { get; private set; } = 100;
    public float CurrenHealth { get; set; } = 100;

    public event Action<float, float> OnHealthChanged;
    public event Action OnPlayerDied;

    void Start()
    {
        CurrenHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrenHealth, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (CurrenHealth - damage >= 0)
        {
            CurrenHealth -= damage;
            OnHealthChanged?.Invoke(CurrenHealth, MaxHealth);
        }
        else
        {
            CurrenHealth = 0;
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        CurrenHealth = Mathf.Min(CurrenHealth + healAmount, MaxHealth);
        OnHealthChanged?.Invoke(CurrenHealth, MaxHealth);
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
        // Handle player death (e.g., play animation, disable controls, etc.)
    }
}
