using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour
{
    public Transform player;
    public Transform returnPosition;
    public float speed = 5f;
    public float shootDistance = 10f;
    public float delayBetweenShots = 2f;
    public float rotationSpeed = 5f;
    public float rotationOffset = 0f; // Adjustment angle for fine-tuning
    public float returnSpeedMultiplier = 2f; // Multiplier for return speed

    private Vector2 shootTargetPosition;
    public bool isShooting = false;
    public bool isReturning = false;
    private Quaternion originalRotation;
    private float originalYRotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalRotation = transform.rotation; // Save the original rotation
        originalYRotation = transform.eulerAngles.y; // Save the original Y rotation
    }

    void Update()
    {
        if (isShooting)
        {
            AimTowards(shootTargetPosition);
            MoveTowards(shootTargetPosition, speed);
        }
        else if (isReturning)
        {
            ResetRotation();
            MoveTowards(returnPosition.position, speed * returnSpeedMultiplier);
        }
    }

    public void StartShooting()
    {
        if (!isShooting && !isReturning)
        {
            isShooting = true;
            CalculateShootTarget();
        }
    }

    private void CalculateShootTarget()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        shootTargetPosition = (Vector2)player.position + directionToPlayer * shootDistance;
    }

    private void MoveTowards(Vector2 targetPosition, float moveSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (isShooting)
            {
                isShooting = false;
                isReturning = true;
            }
            else if (isReturning)
            {
                isReturning = false;
            }
        }
    }

    private void AimTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion targetRotation = Quaternion.Euler(0, originalYRotation, angle); // Preserve Y rotation, adjust Z rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100);
    }

    private void ResetRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, originalYRotation, originalRotation.eulerAngles.z); // Preserve Y rotation, reset to original Z rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100);
    }
}
