using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 9;
    public List<ItemSlot> items = new List<ItemSlot>();

    private void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlot());
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == null)
            {
                items[i].item = item;
                items[i].amount = 1;
                return true;
            }
        }
        return false;
    }

    public Item GetItem(int slot)
    {
        if (slot >= 0 && slot < items.Count)
        {
            return items[slot].item;
        }
        return null;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
            {
                items[i].item = null;
                items[i].amount = 0;
                break;
            }
        }
    }

    [System.Serializable]
    public class ItemSlot
    {
        public Item item;
        public int amount;

        public ItemSlot()
        {
            item = null;
            amount = 0;
        }
    }
}
