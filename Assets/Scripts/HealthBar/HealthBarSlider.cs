using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider component

    // Method to update the health bar
    public void SetHealth(float healthPercentage)
    {
        healthSlider.value = healthPercentage;
    }
}
