using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public event Action<float, float> OnHealthChanged;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        currentHealth = Mathf.Max(maxHealth, 0f);
    }

    private void Start()
    {
        NotifyHealthChanged();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        NotifyHealthChanged();
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
