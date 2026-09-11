using UnityEngine;
using UnityEngine.InputSystem;

public class AttackLaser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject laserBox;

    private InputAction attackAction;

    private void Awake()
    {
        attackAction = inputActions != null
            ? inputActions.FindActionMap("Player")?.FindAction("Attack")
            : null;

        if (laserBox != null)
            laserBox.SetActive(false);
    }

    private void OnEnable()
    {
        if (attackAction == null) return;
        attackAction.Enable();
        attackAction.performed += OnAttackPerformed;
        attackAction.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        if (attackAction == null) return;
        attackAction.performed -= OnAttackPerformed;
        attackAction.canceled -= OnAttackCanceled;
        attackAction.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (laserBox != null)
            laserBox.SetActive(true);
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        if (laserBox != null)
            laserBox.SetActive(false);
    }
}