using UnityEngine;
using System.Collections;

public class StayWithinRangeState : MonoBehaviour
{
    public float speed = 5f; // Speed of the boss movement
    public float erraticness = 2f; // How erratically the boss moves
    public float innerRange = 5f; // Inner range for the boss to stay within
    public float outerRange = 10f; // Outer range for the boss to stay within
    public HandController leftHand;
    public HandController rightHand;

    private Transform player;
    private IceGolemBossController bossController;
    private Vector2 targetPosition;
    private bool canShoot = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossController = GetComponent<IceGolemBossController>();

        if (leftHand == null)
        {
            Debug.LogError("Left hand is not assigned in the Inspector.");
        }
        if (rightHand == null)
        {
            Debug.LogError("Right hand is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            MoveBoss();
            DrawDebugRaycasts();

            if (canShoot)
            {
                StartCoroutine(ShootHands());
            }
        }
    }

    void MoveBoss()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (distanceToPlayer > outerRange)
        {
            // Move towards the player
            targetPosition = (Vector2)player.position;
        }
        else if (distanceToPlayer < innerRange)
        {
            // Move away from the player
            targetPosition = (Vector2)transform.position - directionToPlayer;
        }
        else
        {
            // Move around the player in a circle
            Vector2 perpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);
            Vector2 randomOffset = perpendicularDirection * Random.Range(-erraticness, erraticness);
            targetPosition = (Vector2)player.position + randomOffset;
        }

        // Move the boss towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private IEnumerator ShootHands()
    {
        canShoot = false;

        if (leftHand != null)
        {
            leftHand.StartShooting();
            yield return new WaitUntil(() => !leftHand.isShooting && !leftHand.isReturning);
            yield return new WaitForSeconds(leftHand.delayBetweenShots);
        }
        else
        {
            Debug.LogWarning("Left hand is not assigned.");
        }

        if (rightHand != null)
        {
            rightHand.StartShooting();
            yield return new WaitUntil(() => !rightHand.isShooting && !rightHand.isReturning);
            yield return new WaitForSeconds(rightHand.delayBetweenShots);
        }
        else
        {
            Debug.LogWarning("Right hand is not assigned.");
        }

        canShoot = true;
    }

    void DrawDebugRaycasts()
    {
        // Draw the inner range raycast in red
        Debug.DrawRay(transform.position, (player.position - transform.position).normalized * innerRange, Color.red);

        // Draw the outer range raycast in green
        Debug.DrawRay(transform.position, (player.position - transform.position).normalized * outerRange, Color.green);
    }
}
