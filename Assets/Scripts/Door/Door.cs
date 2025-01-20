
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Door : MonoBehaviour
//{
//    public int level = 2;
//    public int health;
//    public string requiredKeyName = "DoorKey";

//    private void Start()
//    {
//        SetHealthBasedOnLevel();
//    }

//    private void SetHealthBasedOnLevel()
//    {
//        // Adjust this formula as needed
//        health = level; // Example: health is five times the level value
//    }

//    public void TakeDamage(int damage)
//    {
//        health -= damage;
//        Debug.Log($"Door took {damage} damage. Remaining health: {health}");

//        if (health <= 0)
//        {
//            OpenDoor();
//        }
//    }

//    private void OpenDoor()
//    {
//        Debug.Log("Door opened!");
//        gameObject.SetActive(false); // Disable the door
//        LoadNextLevel();
//    }

//    private void LoadNextLevel()
//    {
//        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
//        int totalScenes = SceneManager.sceneCountInBuildSettings;
//        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

//        SceneManager.LoadScene(nextSceneIndex);
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int health;
    public string requiredKeyName = "DoorKey";

    private void Start()
    {
        SetHealthBasedOnCurrentScene();
    }

    private void SetHealthBasedOnCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        health = currentSceneIndex + 1; // Adding 1 because scene indices start at 0
        Debug.Log($"Door health set to {health} based on current scene index");
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
        gameObject.SetActive(false); // Disable the door
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;

        SceneManager.LoadScene(nextSceneIndex);
    }
}
