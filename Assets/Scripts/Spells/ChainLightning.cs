using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class ChainLightning : MonoBehaviour
{
    public SpellScriptableObject spellData;
    private List<GameObject> hitTargets = new List<GameObject>();

    void Start()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        // Stretch towards the initial target (mouse position)
        StretchToTarget(transform.position, mousePosition);
        Destroy(gameObject, spellData.LifeSpan);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            hitTargets.Add(other.gameObject);
            CharacterHealth enemyHealth = other.GetComponent<CharacterHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spellData.DamageAmount);
            }

            StartCoroutine(ChainToNextTarget(other.transform.position));
        }
    }

    private IEnumerator ChainToNextTarget(Vector3 currentTargetPosition)
    {
        yield return new WaitForSeconds(0.1f); // Small delay between chains

        Collider2D[] nearbyTargets = Physics2D.OverlapCircleAll(currentTargetPosition, spellData.chainRadius);
        foreach (var target in nearbyTargets)
        {
            if (target.CompareTag("Enemy") && !hitTargets.Contains(target.gameObject))
            {
                hitTargets.Add(target.gameObject);
                CharacterHealth enemyHealth = target.GetComponent<CharacterHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(spellData.DamageAmount);
                }

                // Instantiate the next chain lightning bolt
                GameObject nextBolt = Instantiate(spellData.spellPrefab, currentTargetPosition, Quaternion.identity);
                ChainLightning nextChain = nextBolt.GetComponent<ChainLightning>();
                nextChain.spellData = spellData;
                nextChain.hitTargets = new List<GameObject>(hitTargets);

                // Stretch the next bolt towards the next target
                nextChain.StretchToTarget(currentTargetPosition, target.transform.position);

                if (hitTargets.Count >= spellData.numberOfChainHits)
                {
                    Destroy(gameObject); // End the chain after the specified number of hits
                }

                break; // Chain to only one target at a time
            }
        }
    }

    private void StretchToTarget(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, endPosition);

        // Set the position to the midpoint between the start and end positions
        transform.position = (startPosition + endPosition) / 2;

        // Set the rotation to align with the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Adjust the scale to stretch the lightning bolt
        transform.localScale = new Vector3(transform.localScale.x, distance, transform.localScale.z);
    }
}
