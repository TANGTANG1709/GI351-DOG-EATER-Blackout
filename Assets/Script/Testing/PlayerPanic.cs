using UnityEngine;

public class PlayerPanic : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float panicStartDistance = 6f;
    [SerializeField] private float maximumPanicDistance = 1.5f;
    [SerializeField] private float scanInterval = 0.1f;

    [Header("Response")]
    [SerializeField] private float panicSmoothing = 2.5f;
    [SerializeField] private PanicCameraShake cameraShake;

    public float CurrentPanic { get; private set; }

    private float targetPanic;
    private float scanTimer;

    private void Awake()
    {
        if (cameraShake == null && Camera.main != null)
        {
            cameraShake = Camera.main.GetComponent<PanicCameraShake>();

            if (cameraShake == null)
                cameraShake = Camera.main.gameObject.AddComponent<PanicCameraShake>();
        }

    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer <= 0f)
        {
            scanTimer = scanInterval;
            targetPanic = CalculateTargetPanic();
        }

        CurrentPanic = Mathf.MoveTowards(
            CurrentPanic,
            targetPanic,
            panicSmoothing * Time.deltaTime
        );

        cameraShake?.SetIntensity(CurrentPanic);
    }

    private float CalculateTargetPanic()
    {
        float nearestDistance = float.MaxValue;
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            if (enemy == null)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            nearestDistance = Mathf.Min(nearestDistance, distance);
        }

        if (nearestDistance == float.MaxValue || nearestDistance >= panicStartDistance)
            return 0f;

        float panic = Mathf.InverseLerp(
            panicStartDistance,
            maximumPanicDistance,
            nearestDistance
        );

        return Mathf.Clamp01(panic);
    }

    private void OnDisable()
    {
        cameraShake?.SetIntensity(0f);
    }
}
