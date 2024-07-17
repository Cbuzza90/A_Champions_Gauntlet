using UnityEngine;

public class DemonBatAI : MonoBehaviour
{
    public float moveSpeed = 2f; // Movement speed of the bat
    public float attackRange = 1f; // Range within which the bat attacks the player
    public float attackCooldown = 1.5f; // Cooldown time between attacks
    public int attackDamage = 10; // Damage dealt by the bat's attack
    public int experiencePoints = 5; // Experience points given when defeated

    private Transform playerTransform; // Reference to the player's transform
    private float attackTimer = 0f; // Timer to handle attack cooldown

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        MoveTowardsPlayer(distanceToPlayer);
        attackTimer -= Time.deltaTime; // Decrease the attack timer
    }

    void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > attackRange)
        {
            // Move towards the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Attack the player if within range and cooldown is over
            if (attackTimer <= 0f)
            {
                AttackPlayer();
                attackTimer = attackCooldown; // Reset the attack timer
            }
        }
    }

    void AttackPlayer()
    {
        // Perform attack logic here
        Debug.Log("Demon Bat attacks the player!");

        // Assuming the player has a PlayerHealth component
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public int GetExperiencePoints()
    {
        return experiencePoints;
    }
}
