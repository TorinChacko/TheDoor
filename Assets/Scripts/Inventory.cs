// Inventory.cs
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 20;
    public List<ItemSlot> items = new List<ItemSlot>();

    private void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlot());
        }
    }

    public bool AddItem(Item item, int amount = 1)
    {
        if (item.isStackable)
        {
            ItemSlot existingSlot = items.Find(slot => slot.item == item && slot.amount < item.maxStackSize);
            if (existingSlot != null)
            {
                existingSlot.amount += amount;
                return true;
            }
        }

        ItemSlot emptySlot = items.Find(slot => slot.item == null);
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.amount = amount;
            return true;
        }

        return false;
    }

    public void RemoveItem(Item item, int amount = 1)
    {
        ItemSlot slot = items.Find(s => s.item == item);
        if (slot != null)
        {
            slot.amount -= amount;
            if (slot.amount <= 0)
            {
                slot.item = null;
                slot.amount = 0;
            }
        }
    }
}

[System.Serializable]
public class ItemSlot
{
    public Item item;
    public int amount;
}
