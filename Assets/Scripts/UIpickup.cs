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
        CreateSlots();
        UpdateUI();
    }

    void CreateSlots()
    {
        for (int i = 0; i < playerInventory.inventorySize; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsParent);
            slotObjects.Add(slotObj);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            Inventory.ItemSlot slot = playerInventory.items[i];
            GameObject slotObj = slotObjects[i];
            Image icon = slotObj.transform.Find("Icon").GetComponent<Image>();
            Text amountText = slotObj.transform.Find("Amount").GetComponent<Text>();

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
            slotObj.GetComponent<Image>().color = (i == playerController.currentSlot) ? Color.yellow : Color.white;
        }
    }
}
