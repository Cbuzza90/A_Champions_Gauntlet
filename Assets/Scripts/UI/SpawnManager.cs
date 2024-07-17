using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawningPortalPrefab; // Reference to the SpawningPortal prefab
    public Transform topLeft; // Reference to the TopLeft corner
    public Transform topRight; // Reference to the TopRight corner
    public Transform bottomLeft; // Reference to the BottomLeft corner
    public Transform bottomRight; // Reference to the BottomRight corner

    private void Start()
    {
        // Optionally validate if the corner references are set
        if (topLeft == null || topRight == null || bottomLeft == null || bottomRight == null)
        {
            Debug.LogError("Corner references are not set in the SpawnManager.");
        }
    }

    public void SpawnPortal()
    {
        Vector2 spawnPosition = GetRandomPositionWithinBounds();
        Instantiate(spawningPortalPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetRandomPositionWithinBounds()
    {
        float x = Random.Range(bottomLeft.position.x, bottomRight.position.x);
        float y = Random.Range(bottomLeft.position.y, topLeft.position.y);

        Vector2 randomPosition = new Vector2(x, y);
        Debug.Log($"Generated Position: {randomPosition} within bounds: ({bottomLeft.position}), ({topRight.position})");

        return randomPosition;
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnPortal", 0f, 5f); // Spawns a portal every second
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnPortal");
    }

    public void IncreaseDifficulty()
    {
        // Logic for increasing difficulty
    }
}
