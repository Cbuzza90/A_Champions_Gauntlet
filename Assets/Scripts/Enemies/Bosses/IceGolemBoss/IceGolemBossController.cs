using UnityEngine;

public class IceGolemBossController : MonoBehaviour
{
    public float health = 100f;
    private StayWithinRangeState stayWithinRangeState;

    void Start()
    {
        stayWithinRangeState = GetComponent<StayWithinRangeState>();
    }

    void Update()
    {
        // Here you can manage transitions between states if needed
        // For now, we just enable the StayWithinRange state
        stayWithinRangeState.enabled = true;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle boss death (e.g., play animation, destroy object, etc.)
        Destroy(gameObject);
    }

    public void StayWithinRangeStateEnabled(bool enabled)
    {
        stayWithinRangeState.enabled = enabled;
    }
}
