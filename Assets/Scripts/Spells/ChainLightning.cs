using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    public SpellScriptableObject spellData;
    public int numberOfChainHits = 4;
    public float chainRadius = 5f;
    public GameObject lightningPrefab; // Prefab of the lightning effect

    private Vector3 initialPosition;
    private Vector3 direction;
    private Rigidbody2D rb;
    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;

        // Calculate initial direction towards the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        direction = (mousePosition - initialPosition).normalized;
        rb.velocity = direction * spellData.Speed; // Initial velocity towards the target

        Debug.Log("ChainLightning Initial Position: " + initialPosition);
        Debug.Log("Mouse Position: " + mousePosition);
        Debug.Log("ChainLightning Direction: " + direction);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            ChainLightningHit(other.transform);
        }
    }

    void ChainLightningHit(Transform target)
    {
        StartCoroutine(ChainLightningEffect(target));
    }

    IEnumerator ChainLightningEffect(Transform target)
    {
        int chainHits = 0;
        Transform currentTarget = target;

        while (chainHits < numberOfChainHits)
        {
            if (currentTarget == null)
                yield break;

            // Apply damage to the current target
            CharacterHealth enemyHealth = currentTarget.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spellData.DamageAmount, Vector3.zero, 0);
            }

            // Find the next target within the chain radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(currentTarget.position, chainRadius);
            Transform nextTarget = null;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") && collider.transform != currentTarget)
                {
                    nextTarget = collider.transform;
                    break;
                }
            }

            // Create lightning effect between targets
            if (nextTarget != null)
            {
                GameObject lightning = Instantiate(lightningPrefab, currentTarget.position, Quaternion.identity);
                // Adjust the lightning's position, rotation, and scale to connect currentTarget and nextTarget
                Vector3 direction = (nextTarget.position - currentTarget.position).normalized;
                float distance = Vector3.Distance(currentTarget.position, nextTarget.position);
                lightning.transform.right = direction;
                lightning.transform.localScale = new Vector3(distance, lightning.transform.localScale.y, lightning.transform.localScale.z);
                Destroy(lightning, 0.5f); // Destroy the lightning effect after a short duration
            }

            currentTarget = nextTarget;
            chainHits++;
            yield return new WaitForSeconds(0.1f); // Short delay between chain hits
        }

        // Destroy the initial lightning bolt after chaining is done
        Destroy(gameObject);
    }
}
