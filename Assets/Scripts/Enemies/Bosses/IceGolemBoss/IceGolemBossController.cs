using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IceGolemBossController : MonoBehaviour
{
    public float maxHealth = 3500f;
    private float currentHealth;
    public string bossName = "Ice Golem";

    public Slider healthBarSlider;
    public TMP_Text bossNameText;
    public TMP_Text bossHealthPoolText;

    public GameObject iceBallPrefab;
    public int numberOfIceBalls = 8;
    public float maxRadius = 5f;
    public float minRadius = 2f;
    public float respawnDelay = 30f;  // Time before respawn after death

    // New variables for speed control
    public float rotationSpeed = 30f;  // Default rotation speed
    public float radiusChangeSpeed = 0.2f;  // Default radius change speed

    private List<GameObject> iceBalls = new List<GameObject>();
    private Queue<GameObject> iceBallPool = new Queue<GameObject>();

    private bool specialAttackTriggered = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        bossNameText.text = bossName;
        UpdateHealthPoolText();            
        
    }

    private void Update()
    {
        // Check if current health is below 50% and special attack hasn't been triggered
        if (currentHealth <= maxHealth / 2 && !specialAttackTriggered)
        {
            InitializeIceBalls();
            specialAttackTriggered = true; // Set the flag to true to avoid re-triggering
        }
    }

    private void InitializeIceBalls()
    {
        for (int i = 0; i < numberOfIceBalls; i++)
        {
            GameObject iceBall = Instantiate(iceBallPrefab, Vector3.zero, Quaternion.identity, transform);
            iceBall.SetActive(false);
            iceBalls.Add(iceBall);
            iceBallPool.Enqueue(iceBall);
        }

        StartCoroutine(SpawnAndManageIceBalls());
    }


    private IEnumerator SpawnAndManageIceBalls()
    {
        int currentBallIndex = 0;
        while (currentBallIndex < numberOfIceBalls)
        {
            if (iceBallPool.Count > 0)
            {
                GameObject iceBall = iceBallPool.Dequeue();
                float angle = currentBallIndex * 2 * Mathf.PI / numberOfIceBalls;
                Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * minRadius;
                iceBall.transform.position = spawnPosition;
                iceBall.SetActive(true);

                IceBall iceBallScript = iceBall.GetComponent<IceBall>();
                iceBallScript.Initialize(transform, maxRadius, minRadius, radiusChangeSpeed, rotationSpeed);

                currentBallIndex++;
                yield return new WaitForSeconds(respawnDelay / numberOfIceBalls);  // Gradual spawning
            }
            else
            {
                yield return null;  // Wait until an ice ball is available in the pool
            }
        }
    }

    public void RespawnIceBall(GameObject iceBall)
    {
        iceBall.SetActive(false);
        iceBallPool.Enqueue(iceBall);
        StartCoroutine(RespawnIceBallAfterDelay(respawnDelay));
    }

    private IEnumerator RespawnIceBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (iceBallPool.Count > 0)
        {
            GameObject iceBall = iceBallPool.Dequeue();
            float angle = Random.Range(0f, 2f * Mathf.PI);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * minRadius;
            iceBall.transform.position = spawnPosition;
            iceBall.SetActive(true);

            IceBall iceBallScript = iceBall.GetComponent<IceBall>();
            iceBallScript.Initialize(transform, maxRadius, minRadius, radiusChangeSpeed, rotationSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        healthBarSlider.value = currentHealth;
        UpdateHealthPoolText();
    }

    private void UpdateHealthPoolText()
    {
        bossHealthPoolText.text = $"{currentHealth} / {maxHealth}";
    }

    private void Die()
    {
        StopAllCoroutines();
        foreach (GameObject iceBall in iceBalls)
        {
            Destroy(iceBall);
        }
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }

        UpdateHealthPoolText();
    }
}
