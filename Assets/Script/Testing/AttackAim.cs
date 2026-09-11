using UnityEngine;

public class AttackAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 15f;

    private Plane groundPlane;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        AimAttackPointAtMouse();
    }

    private void AimAttackPointAtMouse()
    {
        groundPlane = new Plane(Vector3.up, attackPoint.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePos = ray.GetPoint(distance);

            Vector3 direction = worldMousePos - attackPoint.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up)
                                           * Quaternion.Euler(0f, -90f, 0f);

                attackPoint.rotation = Quaternion.Slerp(
                    attackPoint.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}