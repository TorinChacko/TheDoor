using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public float pickupRange = 2f;
    public int currentSlot = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrentItem();
        }

        // Switch items with number keys 1-9
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentSlot = i;
                inventoryUI.UpdateUI();
            }
        }
    }

    void TryPickupItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (Collider2D collider in colliders)
        {
            ItemPickup itemPickup = collider.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                if (inventory.AddItem(itemPickup.item))
                {
                    Destroy(collider.gameObject);
                    inventoryUI.UpdateUI();
                    break;
                }
            }
        }
    }

    void DropCurrentItem()
    {
        Item itemToDrop = inventory.GetItem(currentSlot);
        if (itemToDrop != null)
        {
            inventory.RemoveItem(itemToDrop);
            // Instantiate dropped item prefab here
            // You'll need to create a prefab for dropped items
            GameObject droppedItem = Instantiate(itemToDrop.dropPrefab, transform.position, Quaternion.identity);
            inventoryUI.UpdateUI();
        }
    }
}
