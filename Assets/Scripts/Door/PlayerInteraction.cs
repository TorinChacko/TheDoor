using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    public Inventory inventory;
    public int keyDamage = 10;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Door") && Input.GetKeyDown(KeyCode.E))
        {
            Door door = other.GetComponent<Door>();
            if (door != null && inventory.HasKey(door.requiredKeyName))
            {
                door.TakeDamage(keyDamage);
            }
        }
    }
}
