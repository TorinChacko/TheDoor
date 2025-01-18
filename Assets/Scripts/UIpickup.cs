using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemSlot slot in playerInventory.items)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsParent);
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
        }
    }
}
