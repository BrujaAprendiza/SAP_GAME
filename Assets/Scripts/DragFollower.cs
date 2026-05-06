using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragFollower : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPosition;

    void Awake()
    {
        startPosition = transform.position;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        // Optional: Change color or scale to indicate dragging started
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert screen position to Canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
    }
}