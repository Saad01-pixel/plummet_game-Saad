using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Energy var
    public int maxEnergy = 1000;
    public int minEnergy = 0;
    private int currentEnergy;

    // Collision count
    private int collisions;

    // Wall count
    private int wallCount;

    // UI 
    public Text energyText;
    public Text collisionText;
    public Text wallText;

    private Player player;  // L Player 

    void Start()
    {
        // Initisalization
        player = FindObjectOfType<Player>();
        currentEnergy = maxEnergy;
        collisions = player.playerData.collisions;  
        wallCount = GameObject.FindGameObjectsWithTag("Wall").Length; // How many walls in Scene

        UpdateUI();
    }

    void Update()
    {
        // Player collide = Decrease energy and increment collisions
        if (player.playerData.collisions > collisions)
        {
            collisions = player.playerData.collisions;
            HandleCollision();
        }

        // Update UI
        UpdateUI();
    }

    // Energy decrease
    void HandleCollision()
    {
        // Whe ncollision happens dcecrease
        currentEnergy -= 50;
        if (currentEnergy < minEnergy)
        {
            currentEnergy = minEnergy;
        }

    }

    // Le UI
    void UpdateUI()
    {
        energyText.text = "Energy: " + currentEnergy;
        collisionText.text = "Collisions: " + collisions;
        wallText.text = "Walls that are Keft: " + wallCount;
        scoreText.text = "Score: " + CalculateScore();
    }


    // Wall count
    public void DecreaseWallCount()
    {
        wallCount--;
        if (wallCount <= 0)
        {
            EndGame();
        }
    }

    public void UpdateScore(float newScore)
    {
        // Update le score dans le score manager
       
        Debug.Log("Updated Score: " + newScore);
    }


    void EndGame()
    {
        Debug.Log("Game Over: OOF!");
        // Game over
    }

    // Player + Save data/Load Data
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
