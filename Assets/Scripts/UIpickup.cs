using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;
    public PlayerController playerController;

    private List<GameObject> slotObjects = new List<GameObject>();

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogError("InventoryUI: Player Inventory is not assigned!");
            return;
        }

        if (itemsParent == null)
        {
            Debug.LogError("InventoryUI: Items Parent is not assigned!");
            return;
        }

        if (itemSlotPrefab == null)
        {
            Debug.LogError("InventoryUI: Item Slot Prefab is not assigned!");
            return;
        }

        CreateSlots();
        UpdateUI();
    }

    void CreateSlots()
    {
        // Clear existing slots
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        slotObjects.Clear();

        // Create new slots
        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsParent);
            slotObjects.Add(slotObj);

            // Verify the instantiated slot
            if (slotObj.transform.Find("Icon") == null)
                Debug.LogError($"InventoryUI: 'Icon' child not found in instantiated slot {i}");
            if (slotObj.transform.Find("Amount") == null)
                Debug.LogError($"InventoryUI: 'Amount' child not found in instantiated slot {i}");
        }
    }

    public void UpdateUI()
    {
        if (playerInventory == null)
        {
            Debug.LogError("InventoryUI: playerInventory is null");
            return;
        }

        if (slotObjects == null || slotObjects.Count == 0)
        {
            Debug.LogError("InventoryUI: slotObjects is null or empty");
            return;
        }

        for (int i = 0; i < playerInventory.items.Count && i < slotObjects.Count; i++)
        {
            GameObject slotObj = slotObjects[i];
            if (slotObj == null)
            {
                Debug.LogError($"InventoryUI: Slot object at index {i} is null");
                continue;
            }

            Transform iconTransform = slotObj.transform.Find("Icon");
            Transform amountTransform = slotObj.transform.Find("Amount");

            if (iconTransform == null)
            {
                Debug.LogError($"InventoryUI: 'Icon' child not found in slot {i}. Children: {string.Join(", ", from Transform child in slotObj.transform select child.name)}");
                continue;
            }

            if (amountTransform == null)
            {
                Debug.LogError($"InventoryUI: 'Amount' child not found in slot {i}. Children: {string.Join(", ", from Transform child in slotObj.transform select child.name)}");
                continue;
            }

            Image icon = iconTransform.GetComponent<Image>();
            TextMeshProUGUI amountText = amountTransform.GetComponent<TextMeshProUGUI>();

            if (icon == null)
            {
                Debug.LogError($"InventoryUI: Image component not found on 'Icon' in slot {i}");
                continue;
            }

            if (amountText == null)
            {
                Debug.LogError($"InventoryUI: TextMeshProUGUI component not found on 'Amount' in slot {i}");
                continue;
            }

            Inventory.ItemSlot slot = playerInventory.items[i];

            if (slot.item != null)
            {
                icon.sprite = slot.item.icon;
                icon.enabled = true;
                amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
                amountText.enabled = slot.amount > 1;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
                amountText.text = "";
                amountText.enabled = false;
            }

            // Highlight current slot
            if (playerController != null)
            {
                slotObj.GetComponent<Image>().color = (i == playerController.currentSlot) ? Color.yellow : Color.white;
            }
        }
    }
}
