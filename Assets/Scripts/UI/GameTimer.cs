using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the UI Text element
    public float timeUntilBoss = 120f; // Time in seconds until the boss fight
    public float difficultyIncreaseInterval = 30f; // Interval in seconds to increase difficulty
    public GameObject bossPrefab; // Reference to the boss prefab
    public Transform bossSpawnLocation; // Where the boss will spawn
    public SpawnManager spawnManager; // Reference to your spawn manager
    private float elapsedTime = 0f;
    private float difficultyTimer = 0f;
    private bool bossFightActive = false;

    void Start()
    {
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!bossFightActive)
        {
            elapsedTime += Time.deltaTime;
            difficultyTimer += Time.deltaTime;

            if (elapsedTime >= timeUntilBoss)
            {
                StartBossFight();
            }
            else if (difficultyTimer >= difficultyIncreaseInterval)
            {
                IncreaseDifficulty();
                difficultyTimer = 0f;
            }

            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        float remainingTime = Mathf.Max(0, timeUntilBoss - elapsedTime);
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void IncreaseDifficulty()
    {
        spawnManager.IncreaseDifficulty();
        Debug.Log("Difficulty Increased");
    }

    void StartBossFight()
    {
        bossFightActive = true;
        spawnManager.StopSpawning(); // Stop spawning regular enemies
        DestroyAllEnemies();
        Instantiate(bossPrefab, bossSpawnLocation.position, bossSpawnLocation.rotation);
        Debug.Log("Boss Fight Started");
    }

    void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
