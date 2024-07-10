using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public float damageAmount = 10f; // Set the player's damage amount
    public float knockBackForce = 1f; // Set the knockback force

    // Method to get the damage amount
    public float GetDamage()
    {
        return damageAmount;
    }
}
