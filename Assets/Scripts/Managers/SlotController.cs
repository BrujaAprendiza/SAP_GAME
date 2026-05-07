using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] public string ingredientTag;
    [SerializeField] private int slotNumber;
    [SerializeField] public int ingredientAmmountInSlot = 0;


    private void Awake()
    {
        ingredientAmmountInSlot = 0;
    }

    public void CheckIngredient(GameObject ingredientGO)
    {
        

        if (ingredientGO.tag == ingredientTag & ingredientAmmountInSlot < 3)
        {
            sendToManager(slotNumber);
            
            
            ingredientGO.transform.position = this.transform.position;
            ingredientGO.GetComponent<DragFollower>().enabled = false; // prevent further dragging
        }
        else if (ingredientGO.tag != ingredientTag)
        {
            print("Wrong ingredient for slot " + slotNumber);
        }
        else if(ingredientAmmountInSlot >= 3)
        {
            print("Slot is Full!");
        }
    }

    private void sendToManager(int slotValue)
    {
        
        GameManager.Instance.AddToSlot(slotValue);
    }
}