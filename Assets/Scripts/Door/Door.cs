using UnityEngine;

public class Door : MonoBehaviour
{
    public int level = 1;
    public int health;
    public string requiredKeyName = "DoorKey";

    private void Start()
    {
        SetHealthBasedOnLevel();
    }

    private void SetHealthBasedOnLevel()
    {
        health = level * 50; // Adjust this formula as needed
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Door took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Door opened!");
        gameObject.SetActive(false); // For simplicity, just disable the door
    }
}
