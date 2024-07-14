using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SpellScriptableObject spellData;

    private Rigidbody2D rb;
    private Animator animator;
    private bool hasExploded = false; // To check if the fireball has already exploded

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, spellData.LifeSpan); // Destroy fireball after its lifespan

        // Debugging: Check if components are assigned
        Debug.Log("Fireball Start: Rigidbody2D and Animator components assigned.");
    }

    void Update()
    {
        if (!hasExploded)
        {
            rb.velocity = transform.right * spellData.Speed; // Move the fireball forward
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Fireball collided with " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Fireball collided with Enemy: " + other.name);

            CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                float knockbackForce = 10f; // Adjust this value as needed
                enemyHealth.TakeDamage(spellData.DamageAmount, knockbackDirection, knockbackForce);
            }
            Explode();
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Fireball collided with Ground: " + other.name);
            Explode();
        }
        else
        {
            Debug.Log("Fireball collided with an object not tagged as Enemy or Ground: " + other.tag);
        }
    }

    void Explode()
    {
        if (hasExploded) return; // Prevent multiple calls to Explode

        hasExploded = true;
        // Play explosion animation
        animator.Play("FireballExplosion");
        // Disable the fireball's movement
        rb.velocity = Vector2.zero;     
        // Destroy the fireball after the explosion animation ends
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.2f);
        Debug.Log("animator.GetCurrentAnimatorStateInfo(0).length: " + animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
