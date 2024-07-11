using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PlayerDamage playerDamage;

    private void Start()
    {
        playerDamage = GetComponentInParent<PlayerDamage>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                float damage = playerDamage.GetDamage();
                Vector3 knockbackDirection = playerDamage.CalculateKnockbackDirection(other.transform);
                enemyHealth.TakeDamage(damage, knockbackDirection, playerDamage.knockBackForce);
            }
        }
    }
}
