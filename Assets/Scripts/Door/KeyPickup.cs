using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private Key keyComponent;

    private void Awake()
    {
        keyComponent = GetComponent<Key>();
        if (keyComponent == null)
        {
            Debug.LogError("Key component not found on this object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null && keyComponent != null)
            {
                inventory.AddKey(keyComponent.keyName);
                Destroy(gameObject);
            }
        }
    }
}
