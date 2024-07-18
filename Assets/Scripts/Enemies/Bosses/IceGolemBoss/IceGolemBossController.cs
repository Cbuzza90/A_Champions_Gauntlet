using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IceGolemBossController : MonoBehaviour
{
    public float maxHealth = 1500f;
    private float currentHealth;
    public string bossName = "Ice Golem";

    public Slider healthBarSlider;
    public TMP_Text bossNameText;
    public TMP_Text bossHealthPoolText;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        bossNameText.text = bossName;
        UpdateHealthPoolText();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBarSlider.value = currentHealth;
        UpdateHealthPoolText();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthPoolText()
    {
        bossHealthPoolText.text = $"{currentHealth}/{maxHealth}";
    }

    private void Die()
    {
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
