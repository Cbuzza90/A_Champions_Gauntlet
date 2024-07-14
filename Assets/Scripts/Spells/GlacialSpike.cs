using UnityEngine;

public class GlacialSpike : MonoBehaviour
{
    public SpellScriptableObject spellData;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isCharging = true;
    private bool isShooting = false;
    [SerializeField] private float chargeTimer;
    public Vector3 shootDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        chargeTimer = spellData.glacialSpikeChargeTime; // Assign the charge time from the scriptable object
        shootDirection = transform.right; // Assume initial direction is to the right
        Destroy(gameObject, spellData.LifeSpan); // Destroy fireball after its lifespan

        // Start the charging animation
        animator.Play("GlacialSpikeCharge");
        AdjustAnimationSpeed();
    }

    void Update()
    {
        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                StartShooting();
            }
        }
        else if (isShooting)
        {
            rb.velocity = shootDirection * spellData.Speed; // Move the spike forward
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isShooting) // Only trigger collision handling when shooting
        {
            if (other.CompareTag("Enemy"))
            {
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
                Explode();
            }
        }
    }

    void StartShooting()
    {
        isCharging = false;
        isShooting = true;

        // Start the shooting animation
        animator.Play("GlacialSpikeShooting");
    }

    void Explode()
    {

    }
    void AdjustAnimationSpeed()
    {
        // Get the length of the GlacialSpikeCharge animation
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "GlacialSpikeCharge")
            {
                animator.speed = clip.length / spellData.glacialSpikeChargeTime;
                break;
            }
        }
    }
}
