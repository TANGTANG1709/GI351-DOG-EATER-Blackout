using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    private const string PlayerActionMapName = "Player";
    private const string RechargeActionName = "Recharge";
    private const string ShieldActionName = "Shield";

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private PlayerSanity playerSanity;
    [SerializeField] private GameObject shieldVisual;

    [Header("Shield Settings")]
    [SerializeField] private float shieldSanityCost = 25f;
    [SerializeField] private float shieldDuration = 3f;

    [Header("Shield Flicker")]
    [SerializeField] private float flickerWarningTime = 1f;
    [SerializeField] private float flickerInterval = 0.1f;

    private InputAction rechargeAction;
    private InputAction shieldAction;
    private SpriteRenderer shieldRenderer;
    private bool isRecharging;
    private float shieldTimeRemaining;
    private float flickerTimer;

    private void Awake()
    {
        InputActionMap playerActionMap = inputActions?.FindActionMap(PlayerActionMapName);
        rechargeAction = playerActionMap?.FindAction(RechargeActionName);
        shieldAction = playerActionMap?.FindAction(ShieldActionName);

        if (shieldVisual != null)
            shieldRenderer = shieldVisual.GetComponentInChildren<SpriteRenderer>();

        SetShieldVisible(false);
    }

    private void OnEnable()
    {
        if (rechargeAction != null)
        {
            rechargeAction.Enable();
            rechargeAction.started += OnRechargeStarted;
            rechargeAction.canceled += OnRechargeCanceled;
        }

        if (shieldAction != null)
        {
            shieldAction.Enable();
            shieldAction.performed += OnShieldPerformed;
        }
    }

    private void OnDisable()
    {
        if (rechargeAction != null)
        {
            rechargeAction.started -= OnRechargeStarted;
            rechargeAction.canceled -= OnRechargeCanceled;
            rechargeAction.Disable();
        }

        if (shieldAction != null)
        {
            shieldAction.performed -= OnShieldPerformed;
            shieldAction.Disable();
        }

        isRecharging = false;
        shieldTimeRemaining = 0f;
        flickerTimer = 0f;
        SetShieldVisible(false);
    }

    private void Update()
    {
        if (isRecharging && playerSanity != null)
            playerSanity.Recharge(Time.deltaTime);

        UpdateShieldTimer();
    }

    private void UpdateShieldTimer()
    {
        if (shieldTimeRemaining <= 0f)
            return;

        shieldTimeRemaining -= Time.deltaTime;

        if (shieldTimeRemaining <= 0f)
        {
            SetShieldVisible(false);
            return;
        }

        if (shieldTimeRemaining <= flickerWarningTime)
            UpdateFlicker();
        else if (shieldRenderer != null && !shieldRenderer.enabled)
            shieldRenderer.enabled = true;
    }

    private void UpdateFlicker()
    {
        if (shieldRenderer == null)
            return;

        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            shieldRenderer.enabled = !shieldRenderer.enabled;
            flickerTimer = flickerInterval;
        }
    }

    private void OnRechargeStarted(InputAction.CallbackContext ctx)
    {
        isRecharging = true;
    }

    private void OnRechargeCanceled(InputAction.CallbackContext ctx)
    {
        isRecharging = false;
    }

    private void OnShieldPerformed(InputAction.CallbackContext ctx)
    {
        if (playerSanity == null || !playerSanity.TrySpend(shieldSanityCost))
            return;

        shieldTimeRemaining = shieldDuration;
        flickerTimer = flickerInterval;
        SetShieldVisible(true);
    }

    public bool TryConsumeShield()
    {
        if (shieldTimeRemaining <= 0f)
            return false;

        shieldTimeRemaining = 0f;
        flickerTimer = 0f;
        SetShieldVisible(false);
        return true;
    }

    private void SetShieldVisible(bool isVisible)
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(isVisible);

        if (shieldRenderer != null)
            shieldRenderer.enabled = true;
    }
}