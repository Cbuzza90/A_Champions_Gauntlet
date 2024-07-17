using UnityEngine;

public class PoisonSpear : MonoBehaviour
{
    public SpellScriptableObject spellData; // Details like damage, speed, etc.
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * spellData.Speed; // Assumes it faces the right direction at spawn

        // Destroy the bolt after its lifespan
        Destroy(gameObject, spellData.LifeSpan);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                // Apply initial damage
                enemyHealth.TakeDamage(spellData.DamageAmount);
                // Start poison effect
                enemyHealth.StartCoroutine(enemyHealth.ApplyPoison(spellData.PoisonDamage, spellData.PoisonTicks, spellData.PoisonInterval)); // Poison damage, ticks, interval
            }
        }
    }
}
