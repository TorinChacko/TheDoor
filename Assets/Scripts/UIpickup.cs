using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;
    public PlayerController playerController;

    private List<GameObject> slotObjects = new List<GameObject>();

    private void Start()
    {
        if (playerInventory == null || itemsParent == null || itemSlotPrefab == null)
        {
            Debug.LogError("InventoryUI: Missing references. Please check the Inspector.");
            return;
        }

        CreateSlots();
        UpdateUI();
    }

    void CreateSlots()
    {
        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsParent);
            if (slotObj != null)
            {
                slotObjects.Add(slotObj);
            }
            else
            {
                Debug.LogError($"InventoryUI: Failed to instantiate slot {i}");
            }
        }
    }

    public void UpdateUI()
    {
        if (playerInventory == null || slotObjects == null)
        {
            Debug.LogError("InventoryUI: playerInventory or slotObjects is null");
            return;
        }

        for (int i = 0; i < playerInventory.items.Count && i < slotObjects.Count; i++)
        {
            Inventory.ItemSlot slot = playerInventory.items[i];
            GameObject slotObj = slotObjects[i];

            if (slotObj == null)
            {
                Debug.LogError($"InventoryUI: Slot object at index {i} is null");
                continue;
            }

            Image icon = slotObj.transform.Find("Icon")?.GetComponent<Image>();
            Text amountText = slotObj.transform.Find("Amount")?.GetComponent<Text>();

            if (icon == null || amountText == null)
            {
                Debug.LogError($"InventoryUI: Icon or AmountText not found in slot {i}");
                continue;
            }

            if (slot.item != null)
            {
                icon.sprite = slot.item.icon;
                icon.enabled = true;
                amountText.text = slot.amount.ToString();
            }
            else
            {
                icon.enabled = false;
                amountText.text = "";
            }

            // Highlight current slot
            Image slotImage = slotObj.GetComponent<Image>();
            if (slotImage != null && playerController != null)
            {
                slotImage.color = (i == playerController.currentSlot) ? Color.yellow : Color.white;
            }
        }
    }
}
