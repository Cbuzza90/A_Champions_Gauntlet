using System.Collections;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarPrefab; // Reference to the health bar prefab
    private HealthBarSlider healthBar; // Reference to the health bar script
    private GameObject healthBarObject; // Reference to the instantiated health bar object
    private Rigidbody2D rb; // Rigidbody2D component

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
        // Update the health bar position in FixedUpdate to ensure smooth following
        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.3f, 0));
        healthBarObject.transform.position = healthBarPosition;
    }

    public void TakeDamage(float damage, Vector3 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Manually apply knockback for kinematic Rigidbody2D
        if (rb != null && rb.isKinematic)
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
