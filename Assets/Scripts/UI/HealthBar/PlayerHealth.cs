using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public HealthBarSlider healthBar; // Reference to the HealthBarSlider script
    public ExperienceBar experienceBar; // Reference to the ExperienceBar script

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar
        healthBar.SetHealth(currentHealth / maxHealth);
        Debug.Log("Player Health after damage: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., show game over screen, respawn, etc.)
        Debug.Log("Player has died!");
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar
        healthBar.SetHealth(currentHealth / maxHealth);
    }

    public void GainExperience(int experience)
    {
        experienceBar.AddExperience(experience);
    }
}
