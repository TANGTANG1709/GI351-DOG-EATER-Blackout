using UnityEngine;
using System;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 30f;

    [Header("Hit Effect")]
    [SerializeField] private float flickDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float currentHealth;
    private bool isDead;

    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = Mathf.Max(maxHealth, 0f);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
            return;

        ApplyDamage(amount);
        NotifyHealthChanged();
        StartCoroutine(FlickWhite());

        if (currentHealth <= 0f)
            Die();
    }

    private void ApplyDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator FlickWhite()
    {
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flickDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}