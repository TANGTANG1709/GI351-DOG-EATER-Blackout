using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        playerHealth ??= FindAnyObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void Start()
    {
        if (playerHealth != null)
            HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float current, float max)
    {
        float percentage = max > 0f ? Mathf.Clamp01(current / max) : 0f;

        if (healthFillImage != null)
            healthFillImage.fillAmount = percentage;

        if (healthText != null)
            healthText.text = $"HP {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }
}
