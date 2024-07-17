using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider component

    private void Start()
    {
        if (healthSlider == null)
        {
            Debug.LogError("Slider component not assigned in HealthBarSlider.");
        }
    }

    // Method to update the health bar
    public void SetHealth(float healthPercentage)
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
            Debug.Log("Health Slider Updated: " + healthPercentage);
        }
    }
}
