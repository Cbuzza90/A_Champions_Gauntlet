using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FrostBoomerang : MonoBehaviour
{
    public SpellScriptableObject spellData;
    public float boomerangRange = 10f;

    private Vector3 initialPosition;
    private Vector3 direction;
    private bool returning = false;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private PlayerStateController playerController;

    private static List<FrostBoomerang> activeBoomerangs = new List<FrostBoomerang>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerTransform.GetComponent<PlayerStateController>();
        activeBoomerangs.Add(this);

        if (playerTransform == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found in the scene.");
        }

        // Calculate initial direction towards the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        direction = (mousePosition - initialPosition).normalized;
        rb.velocity = direction * spellData.Speed; // Initial velocity towards the target

        Debug.Log("Boomerang Initial Position: " + initialPosition);
        Debug.Log("Mouse Position: " + mousePosition);
        Debug.Log("Boomerang Direction: " + direction);

        // Reduce player boomerang charges
        playerController.FrostBoomerangeCharges--;
    }

    void Update()
    {
        if (!returning)
        {
            rb.velocity = direction * spellData.Speed;

            if (Vector3.Distance(transform.position, initialPosition) >= boomerangRange)
            {
                returning = true;
                direction = (playerTransform.position - transform.position).normalized;
            }
        }
        else
        {
            if (playerTransform != null)
            {
                direction = (playerTransform.position - transform.position).normalized; // Update direction towards player
                rb.velocity = direction * spellData.Speed;

                if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
                {
                    ReturnToPlayer();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (returning) return;

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
        }
    }

    void Explode()
    {
        returning = true;
        // Play explosion animation or effect
        // Animator or particle system can be triggered here
    }

    void ReturnToPlayer()
    {
        activeBoomerangs.Remove(this);
        playerController.FrostBoomerangeCharges++; // Restore a boomerang charge to the player
        Destroy(gameObject);
    }

    public static bool CanCastBoomerang(PlayerStateController playerController)
    {
        return playerController.FrostBoomerangeCharges > 0;
    }

    public static void CastBoomerang(SpellScriptableObject spell, Vector3 startPosition, Transform playerTransform)
    {
        PlayerStateController playerController = playerTransform.GetComponent<PlayerStateController>();
        if (playerController.FrostBoomerangeCharges > 0)
        {
            GameObject boomerangPrefab = Instantiate(spell.spellPrefab, startPosition, Quaternion.identity);
            FrostBoomerang boomerang = boomerangPrefab.GetComponent<FrostBoomerang>();
            boomerang.spellData = spell;
            boomerang.playerTransform = playerTransform;
        }
    }

}
