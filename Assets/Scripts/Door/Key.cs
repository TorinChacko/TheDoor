using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyName = "DoorKey";
    public int damage = 50000;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (door != null && door.requiredKeyName == keyName)
        {
            door.TakeDamage(damage);
            Destroy(gameObject); // Destroy the key after use
        }
    }
}
