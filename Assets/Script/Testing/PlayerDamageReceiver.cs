using UnityEngine;
using System.Collections;

public class PlayerDamageReceiver : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAbilities playerAbilities;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Hit Effect")]
    [SerializeField] private float flickDuration = 0.1f;

    private void Awake()
    {
        playerHealth ??= GetComponent<PlayerHealth>();
        playerAbilities ??= GetComponent<PlayerAbilities>();
        spriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        if (playerAbilities != null && playerAbilities.TryConsumeShield())
            return;

        playerHealth?.TakeDamage(amount);
        StartCoroutine(FlickWhite());
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
}
