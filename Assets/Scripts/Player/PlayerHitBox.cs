using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PlayerDamage playerDamage;

    private void Start()
    {
        // Assuming the PlayerDamage script is on the parent GameObject
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
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
