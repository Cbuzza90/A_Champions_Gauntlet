using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarPrefab; // Reference to the health bar prefab
    private HealthBarSlider healthBar; // Reference to the health bar script
    private GameObject healthBarObject; // Reference to the instantiated health bar object

    private void Start()
    {
        currentHealth = maxHealth;

        // Instantiate the health bar prefab and set its parent to the Canvas
        healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarObject.transform.SetParent(GameObject.Find("Canvas").transform, false);

        // Get the HealthBarSlider component from the instantiated health bar
        healthBar = healthBarObject.GetComponent<HealthBarSlider>();
    }

    private void Update()
    {
        // Update the health bar position in FixedUpdate to ensure smooth following
        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0));
        healthBarObject.transform.position = healthBarPosition;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar
        healthBar.SetHealth(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle death (destroy the game object, play animation, etc.)
        Destroy(gameObject);
        Destroy(healthBarObject);
    }
}
