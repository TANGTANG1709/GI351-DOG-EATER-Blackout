using UnityEngine;

public class PanicBlackoutUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPanic playerPanic;
    [SerializeField] private RectTransform topEdge;
    [SerializeField] private RectTransform bottomEdge;
    [SerializeField] private RectTransform leftEdge;
    [SerializeField] private RectTransform rightEdge;

    [Header("Blackout")]
    [SerializeField, Range(0f, 0.48f)] private float maximumBlackoutSize = 0.20f;

    private RectTransform[] edges;

    private void Awake()
    {
        edges = new[] { topEdge, bottomEdge, leftEdge, rightEdge };
    }

    private void Update()
    {
        float panic = playerPanic != null ? playerPanic.CurrentPanic : 0f;
        UpdateEdges(panic);
    }

    private void OnDisable()
    {
        UpdateEdges(0f);
    }

    private void UpdateEdges(float panic)
    {
        if (edges == null)
            return;

        float edgeSize = maximumBlackoutSize * Mathf.Clamp01(panic);
        SetEdge(edges[0], new Vector2(0f, 1f - edgeSize), Vector2.one, edgeSize > 0.001f);
        SetEdge(edges[1], Vector2.zero, new Vector2(1f, edgeSize), edgeSize > 0.001f);
        SetEdge(edges[2], new Vector2(0f, edgeSize), new Vector2(edgeSize, 1f - edgeSize), edgeSize > 0.001f);
        SetEdge(edges[3], new Vector2(1f - edgeSize, edgeSize), Vector2.one, edgeSize > 0.001f);
    }

    private static void SetEdge(RectTransform edge, Vector2 anchorMin, Vector2 anchorMax, bool isVisible)
    {
        if (edge == null)
            return;

        edge.anchorMin = anchorMin;
        edge.anchorMax = anchorMax;
        edge.offsetMin = Vector2.zero;
        edge.offsetMax = Vector2.zero;
        edge.gameObject.SetActive(isVisible);
    }
}
