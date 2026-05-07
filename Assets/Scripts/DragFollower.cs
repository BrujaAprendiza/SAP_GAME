using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DragFollower : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform parentRectTransform;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private Canvas parentCanvas;  // Needed to convert screen to local position

    //public bool canSnap = false;
   

    // Assign these in Inspector, or find them dynamically
    [SerializeField] private List<SlotController> allSlots;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

     

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    
        Vector2 newPos = parentRectTransform.InverseTransformVector(eventData.position);
        rectTransform.position = newPos;

    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SlotController targetSlot = GetOverlappingSlot();
        if (targetSlot.ingredientTag == this.tag & targetSlot.ingredientAmmountInSlot < 3)
        {
            // Snap ingredient to slot position
            rectTransform.position = targetSlot.transform.position;

            // Tell the slot we dropped an ingredient
            targetSlot.CheckIngredient(this.gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
        
    }

    private SlotController GetOverlappingSlot()
    {
        foreach (SlotController slot in allSlots)
        {
            if (RectsOverlap(rectTransform, slot.GetComponent<RectTransform>()))
            {
                return slot;
            }
        }
        return null;
    }

    private bool RectsOverlap(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];
        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Rect r1 = new Rect(corners1[0].x, corners1[0].y,
                           corners1[2].x - corners1[0].x,
                           corners1[2].y - corners1[0].y);
        Rect r2 = new Rect(corners2[0].x, corners2[0].y,
                           corners2[2].x - corners2[0].x,
                           corners2[2].y - corners2[0].y);
        return r1.Overlaps(r2);
    }
}