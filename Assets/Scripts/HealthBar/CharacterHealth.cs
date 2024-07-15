using System.Collections;
using UnityEngine;
using TMPro;

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarPrefab; // Reference to the health bar prefab
    public GameObject damageNumberPrefab; // Reference to the damage number prefab

    private HealthBarSlider healthBar; // Reference to the health bar script
    private GameObject healthBarObject; // Reference to the instantiated health bar object
    private Rigidbody2D rb; // Rigidbody2D component
    private Coroutine poisonCoroutine; // To manage poison effect coroutine
    private bool isPoisoned = false; // To track if the character is currently poisoned

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        // Instantiate the health bar prefab and set its parent to the Canvas
        healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarObject.transform.SetParent(GameObject.Find("Canvas").transform, false);

        // Get the HealthBarSlider component from the instantiated health bar
        healthBar = healthBarObject.GetComponent<HealthBarSlider>();
    }

    private void Update()
    {
        // Update the health bar position to ensure smooth following
        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.3f, 0));
        healthBarObject.transform.position = healthBarPosition;
    }

    public void TakeDamage(float damage, Vector3 knockbackDirection = default, float knockbackForce = 0)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Display damage number
        ShowDamageNumber(damage);

        // Manually apply knockback for kinematic Rigidbody2D
        if (rb != null && rb.isKinematic && knockbackForce > 0)
        {
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackForce));
        }

        // Update the health bar
        healthBar.SetHealth(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamageNumber(float damage)
    {
        Debug.Log("Attempting to instantiate damage number prefab.");

        if (damageNumberPrefab == null)
        {
            Debug.LogError("damageNumberPrefab is not assigned in CharacterHealth.");
            return;
        }

        GameObject damageNumberObject = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity);
        Debug.Log("Instantiated damage number prefab.");

        // Set the parent to the canvas
        damageNumberObject.transform.SetParent(GameObject.Find("DamageCanvas").transform, false);

        DamageNumber damageNumber = damageNumberObject.GetComponent<DamageNumber>();
        if (damageNumber == null)
        {
            Debug.LogError("DamageNumber component not found on damageNumberPrefab.");
            Destroy(damageNumberObject);
            return;
        }

        damageNumber.SetDamage(damage);
        damageNumber.SetTarget(transform); // Set the target to this enemy
    }

    public IEnumerator ApplyPoison(float poisonDamage, int ticks, float tickInterval)
    {
        if (isPoisoned)
        {
            yield break; // Prevent multiple poison effects at the same time
        }

        isPoisoned = true;

        // Change color to indicate poison effect
        Color originalColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.green;

        for (int i = 0; i < ticks; i++)
        {
            TakeDamage(poisonDamage);
            yield return new WaitForSeconds(tickInterval);
        }

        // Reset color after poison effect
        GetComponent<SpriteRenderer>().color = originalColor;
        isPoisoned = false;
    }

    private IEnumerator ApplyKnockback(Vector3 direction, float force)
    {
        float duration = 0.2f; // Duration for how long the knockback should last
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            rb.MovePosition(rb.position + (Vector2)(direction.normalized * force * Time.deltaTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Die()
    {
        // Handle death (destroy the game object, play animation, etc.)
        Destroy(gameObject);
        Destroy(healthBarObject);
    }
}
