using UnityEngine;
public class Door : MonoBehaviour
{
    public int health;
    public int level;
    public string requiredKeyName = "Key";

    private void Start()
    {
        SetHealthBasedOnLevel();
    }

    private void SetHealthBasedOnLevel()
    {
        health = level * 50; // Adjust the multiplier as needed
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Implement door opening logic here
        gameObject.SetActive(false); // For simplicity, just disable the door
    }
}
