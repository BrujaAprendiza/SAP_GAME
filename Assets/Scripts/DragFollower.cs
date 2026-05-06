using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragFollower : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private RectTransform parentTransform;
    private Vector2 startPosition;

    void Awake()
    {
        startPosition = transform.position;
        rectTransform = GetComponent<RectTransform>();
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        // Optional: Change color or scale to indicate dragging started
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = parentTransform.InverseTransformVector(eventData.position);
        rectTransform.position = newPos;
        print("Mouse pos: " + eventData.position);
        print("New pos: " + newPos);
    }

    public void Update()
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // rectTransform.position = startPosition;
    }
}