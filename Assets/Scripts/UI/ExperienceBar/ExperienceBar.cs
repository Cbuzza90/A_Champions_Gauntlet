using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider experienceSlider; // Reference to the Slider component
    public int playerLevel = 1; // Initial player level
    public float currentExperience = 0f; // Current experience points
    public float experienceToNextLevel = 100f; // Experience points needed for the next level

    // Method to update the experience bar
    public void AddExperience(float experience)
    {
        currentExperience += experience;
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
        UpdateExperienceBar();
    }

    // Method to handle leveling up
    private void LevelUp()
    {
        playerLevel++;
        currentExperience -= experienceToNextLevel;
        experienceToNextLevel *= 1.5f; // Increase the experience required for the next level
        // Add additional logic for leveling up (e.g., increase stats, unlock abilities, etc.)
    }

    // Method to update the experience bar UI
    private void UpdateExperienceBar()
    {
        experienceSlider.value = currentExperience / experienceToNextLevel;
    }
}
