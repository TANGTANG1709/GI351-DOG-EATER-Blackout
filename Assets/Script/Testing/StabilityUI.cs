using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StabilityUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldStability worldStability;
    [SerializeField] private Image stabilityFillImage;
    [SerializeField] private TMP_Text stabilityText;

    private void Awake()
    {
        worldStability ??= FindAnyObjectByType<WorldStability>();
    }

    private void OnEnable()
    {
        if (worldStability != null)
            worldStability.OnStabilityChanged += HandleStabilityChanged;
    }

    private void Start()
    {
        if (worldStability != null)
            HandleStabilityChanged(worldStability.CurrentStability, worldStability.MaxStability);
    }

    private void OnDisable()
    {
        if (worldStability != null)
            worldStability.OnStabilityChanged -= HandleStabilityChanged;
    }

    private void HandleStabilityChanged(float current, float max)
    {
        float percentage = max > 0f ? Mathf.Clamp01(current / max) : 0f;

        if (stabilityFillImage != null)
            stabilityFillImage.fillAmount = percentage;

        if (stabilityText != null)
            stabilityText.text = $"STABILITY {Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }

}
