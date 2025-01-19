using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemIcon;
    public Button button;

    private Item currentItem;

    private void Awake()
    {
        // Get references if not set in inspector
        if (itemIcon == null)
            itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        if (button == null)
            button = GetComponent<Button>();

        // Add click listener
        button.onClick.AddListener(OnSlotClicked);
    }

    public void UpdateSlot(Item item)
    {
        currentItem = item;

        if (item != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }

    private void OnSlotClicked()
    {
        if (currentItem != null)
        {
            Debug.Log($"Clicked on {currentItem.itemName}");
            // Add your item interaction logic here
        }
        else
        {
            Debug.Log("Clicked on empty slot");
        }
    }
}
