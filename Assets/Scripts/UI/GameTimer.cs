using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the UI Text element
    public float difficultyIncreaseInterval = 30f; // Interval in seconds to increase difficulty
    public SpawnManager spawnManager; // Reference to your spawn manager
    private float elapsedTime = 0f;
    private float difficultyTimer = 0f;

    void Start()
    {
        UpdateTimerDisplay();
        //spawnManager.StartSpawning(); // Start the spawning process
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void IncreaseDifficulty()
    {
        spawnManager.IncreaseDifficulty();
        Debug.Log("Difficulty Increased");
    }
}
