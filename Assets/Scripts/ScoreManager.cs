using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Energy variables
    public int maxEnergy = 1000;
    public int minEnergy = 0;
    private int currentEnergy;

    // Player collision count
    private int collisions;

    // Wall count
    private int wallCount;

    // UI elements for displaying energy, collisions, and walls
    public Text energyText;
    public Text collisionText;
    public Text wallText;

    private Player player;  // Reference to the Player script

    void Start()
    {
        // Initializing the player and energy
        player = FindObjectOfType<Player>();
        currentEnergy = maxEnergy;
        collisions = player.playerData.collisions;  // Start with collisions from PlayerData
        wallCount = GameObject.FindGameObjectsWithTag("Wall").Length; // Count walls in the scene

        UpdateUI();
    }

    void Update()
    {
        // Every time the player collides, decrease energy and increment collisions
        if (player.playerData.collisions > collisions)
        {
            collisions = player.playerData.collisions;
            HandleCollision();
        }

        // Update UI
        UpdateUI();
    }

    // Handle collision logic (energy decrease)
    void HandleCollision()
    {
        // Decrease energy when collision happens
        currentEnergy -= 50;
        if (currentEnergy < minEnergy)
        {
            currentEnergy = minEnergy;
        }

        // Optionally, you could have additional logic for wall interaction
        // (e.g., when all walls are gone, end the game or display a message)
        if (wallCount == 0)
        {
            EndGame();
        }
    }

    // Method to update UI elements
    void UpdateUI()
    {
        energyText.text = "Energy: " + currentEnergy;
        collisionText.text = "Collisions: " + collisions;
        wallText.text = "Walls Remaining: " + wallCount;
    }

    // Method to decrement the wall count
    public void DecreaseWallCount()
    {
        wallCount--;
        if (wallCount <= 0)
        {
            EndGame();
        }
    }

    // Method to end the game when all walls are gone
    void EndGame()
    {
        Debug.Log("Game Over: All walls are gone!");
        // Implement game over logic here
    }

    // Method to interact with the PlayerData to save/load energy, collisions, etc.
    public void SaveGameData()
    {
        player.playerData.collisions = collisions;
        player.playerData.SavePlayerData();
    }

    public void LoadGameData()
    {
        player.playerData.LoadPlayerData();
        collisions = player.playerData.collisions;
    }
}
