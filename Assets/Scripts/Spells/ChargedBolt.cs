using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ChargedBolt : MonoBehaviour
{
    public SpellScriptableObject spellData;
    private Vector3 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        targetPosition.z = 0;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Set initial velocity
        rb.velocity = direction * spellData.Speed;

        // Destroy the bolt after its lifespan
        Destroy(gameObject, spellData.LifeSpan);
    }

    void Update()
    {
        // Add some erratic movement
        rb.velocity += new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spellData.DamageAmount);
            }
            Destroy(gameObject);
        }
    }
}
