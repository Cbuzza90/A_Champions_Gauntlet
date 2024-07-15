using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of different enemy types
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 5f; // Interval between spawns
    private float spawnTimer = 0f;
    private bool isSpawning = true;

    void Update()
    {
        if (isSpawning)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
    }

    public void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(1f, spawnInterval * 0.9f); // Decrease spawn interval by 10%, minimum 1 second
        // Additional difficulty adjustments can be added here
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        isSpawning = true;
    }
}
