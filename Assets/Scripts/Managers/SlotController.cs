using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private string ingredientTag;
    [SerializeField] private int slotNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision != null)
        {
            CheckIngredient(collision.gameObject);
        }

    }

    private void CheckIngredient(GameObject ingredientGO)
    {
        if (ingredientGO.tag == ingredientTag)
        {
            sendToManager(slotNumber);
            ingredientGO.gameObject.gameObject.transform.position = this.transform.position; //This could break without RectTransform
        }
    }


    private void sendToManager(int slotValue)
    {
        GameManager.Instance.AddToSlot(slotValue);
    }
}
