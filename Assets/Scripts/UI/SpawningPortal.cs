using UnityEngine;
using System.Collections;

public class SpawningPortal : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of different enemy types
    public float spawnInterval = 1f; // Interval between spawns
    public float spawnDuration = 15f; // Duration of spawning
    private float spawnTimer = 0f; // Timer to keep track of spawning
    private bool isSpawning = true;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        float endTime = Time.time + spawnDuration;
        while (Time.time < endTime)
        {
            if (isSpawning)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        Destroy(gameObject); // Destroy the portal after spawning is complete
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemyIndex], transform.position, Quaternion.identity);
    }
}
