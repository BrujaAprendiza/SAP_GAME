using UnityEngine;
using UnityEngine.EventSystems;

public class MixFoodManager : MonoBehaviour, IPointerClickHandler
{
    public MixFood spoonMover;

    private void Start()
    {
        print("BBBBB");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        print("Print Registered");
        if (spoonMover != null)
        {
            print("aaa");
            spoonMover.AdvanceToNextWaypoint();
        }
    }
}
