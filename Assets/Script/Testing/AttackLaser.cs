using UnityEngine;
using UnityEngine.InputSystem;

public class AttackLaser : MonoBehaviour
{
    private const string PlayerActionMapName = "Player";
    private const string AttackActionName = "Attack";

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject laserBox;
    [SerializeField] private PlayerSanity playerSanity;

    private InputAction attackAction;
    private bool isAttacking;

    private void Awake()
    {
        InputActionMap playerActionMap = inputActions?.FindActionMap(PlayerActionMapName);
        attackAction = playerActionMap?.FindAction(AttackActionName);
        SetLaserActive(false);
    }

    private void OnEnable()
    {
        if (attackAction == null)
            return;

        attackAction.Enable();
        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.performed -= OnAttackPerformed;
            attackAction.canceled -= OnAttackCanceled;
            attackAction.Disable();
        }

        SetLaserActive(false);
    }

    private void Update()
    {
        if (!isAttacking || playerSanity == null)
            return;

        playerSanity.Drain(Time.deltaTime);

        if (!playerSanity.HasSanity)
            SetLaserActive(false);
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (playerSanity != null && !playerSanity.HasSanity)
            return;

        SetLaserActive(true);
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        SetLaserActive(false);
    }

    private void SetLaserActive(bool isActive)
    {
        isAttacking = isActive;

        if (laserBox != null)
            laserBox.SetActive(isActive);
    }
}