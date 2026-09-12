using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PanicBlackoutUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldStability worldStability;
    [SerializeField] private RectTransform topEdge;
    [SerializeField] private RectTransform bottomEdge;
    [SerializeField] private RectTransform leftEdge;
    [SerializeField] private RectTransform rightEdge;
    [SerializeField] private Volume globalVolume;

    [Header("Blackout")]
    [SerializeField, Range(0f, 0.48f)] private float maximumBlackoutSize = 0.20f;
    [SerializeField, Range(0f, 1f)] private float maximumVignetteIntensity = 0.65f;
    [SerializeField] private float responseSpeed = 8f;

    private RectTransform[] edges;
    private Image[] edgeImages;
    private Vignette vignette;
    private float visualPanic;

    private void Awake()
    {
        edges = new[] { topEdge, bottomEdge, leftEdge, rightEdge };
        edgeImages = new Image[edges.Length];
        for (int i = 0; i < edges.Length; i++)
        {
            if (edges[i] != null)
                edgeImages[i] = edges[i].GetComponent<Image>();
        }

        if (globalVolume == null)
            globalVolume = FindFirstObjectByType<Volume>();

        if (globalVolume != null && globalVolume.profile != null)
            globalVolume.profile.TryGet(out vignette);
    }

    private void Update()
    {
        float instability = worldStability != null ? worldStability.Instability : 0f;
        visualPanic = Mathf.MoveTowards(visualPanic, instability, responseSpeed * Time.deltaTime);
        UpdateEdges(visualPanic);

        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = Mathf.Lerp(0.2f, maximumVignetteIntensity, visualPanic);
            vignette.smoothness.overrideState = true;
            vignette.smoothness.value = Mathf.Lerp(0.2f, 0.55f, visualPanic);
        }
    }

    private void OnDisable()
    {
        UpdateEdges(0f);

        if (vignette != null)
            vignette.intensity.value = 0.2f;
    }

    private void UpdateEdges(float panic)
    {
        if (edges == null)
            return;

        float edgeSize = maximumBlackoutSize * Mathf.Clamp01(panic);
        float edgeAlpha = Mathf.Lerp(0.15f, 0.85f, panic);
        SetEdge(edges[0], edgeImages[0], new Vector2(0f, 1f - edgeSize), Vector2.one, edgeSize > 0.001f, edgeAlpha);
        SetEdge(edges[1], edgeImages[1], Vector2.zero, new Vector2(1f, edgeSize), edgeSize > 0.001f, edgeAlpha);
        SetEdge(edges[2], edgeImages[2], new Vector2(0f, edgeSize), new Vector2(edgeSize, 1f - edgeSize), edgeSize > 0.001f, edgeAlpha);
        SetEdge(edges[3], edgeImages[3], new Vector2(1f - edgeSize, edgeSize), Vector2.one, edgeSize > 0.001f, edgeAlpha);
    }

    private static void SetEdge(RectTransform edge, Image image, Vector2 anchorMin, Vector2 anchorMax, bool isVisible, float alpha)
    {
        if (edge == null)
            return;

        edge.anchorMin = anchorMin;
        edge.anchorMax = anchorMax;
        edge.offsetMin = Vector2.zero;
        edge.offsetMax = Vector2.zero;
        edge.gameObject.SetActive(isVisible);

        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
