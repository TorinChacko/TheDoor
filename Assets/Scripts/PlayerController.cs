using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<ItemPickup>().item;
            if (inventory.AddItem(item))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
