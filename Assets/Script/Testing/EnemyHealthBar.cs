using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Transform fillTransform; // the fill sprite's Transform
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private GameObject barRoot; // parent of background+fill, to hide/show

    private float fullScaleX;

    private void Awake()
    {
        fullScaleX = fillTransform.localScale.x;
    }

    private void OnEnable()
    {
        enemyHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        enemyHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float current, float max)
    {
        float pct = Mathf.Clamp01(current / max);

        Vector3 scale = fillTransform.localScale;
        scale.x = fullScaleX * pct;
        fillTransform.localScale = scale;

        if (hideWhenFull && barRoot != null)
        {
            barRoot.SetActive(pct < 1f);
        }
    }
}