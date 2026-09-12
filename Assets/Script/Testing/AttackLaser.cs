using UnityEngine;
using UnityEngine.InputSystem;

public class AttackLaser : MonoBehaviour
{
    private const string PlayerActionMapName = "Player";
    private const string AttackActionName = "Attack";

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private PlayerSanity playerSanity;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed = 18f;
    [SerializeField] private float projectileDamage = 25f;
    [SerializeField] private float projectileLifetime = 2f;
    [SerializeField] private float fireCooldown = 0.15f;
    [SerializeField] private float sanityCostPerShot = 2f;

    private InputAction attackAction;
    private float nextFireTime;

    private void Awake()
    {
        InputActionMap playerActionMap = inputActions?.FindActionMap(PlayerActionMapName);
        attackAction = playerActionMap?.FindAction(AttackActionName);

        if (projectileSpawnPoint == null)
            projectileSpawnPoint = transform;
    }

    private void OnEnable()
    {
        if (attackAction == null)
            return;

        attackAction.Enable();
        attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.performed -= OnAttackPerformed;
            attackAction.Disable();
        }

    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (Time.time < nextFireTime || projectilePrefab == null || projectileSpawnPoint == null)
            return;

        if (playerSanity == null || !playerSanity.TrySpend(sanityCostPerShot))
            return;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation
        );

        LaserProjectile projectile = projectileObject.GetComponent<LaserProjectile>();
        if (projectile == null)
        {
            Destroy(projectileObject);
            return;
        }

        projectile.Launch(
            projectileSpawnPoint.right,
            projectileSpeed,
            projectileDamage,
            projectileLifetime,
            gameObject
        );

        nextFireTime = Time.time + fireCooldown;
    }
}
