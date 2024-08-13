using UnityEngine;

public class IceBall : MonoBehaviour
{
    public Transform centerTransform; // Center position of the boss
    private float maxRadius;           // Maximum radius
    private float minRadius;           // Minimum radius
    private float radiusChangeSpeed;   // Speed at which radius changes
    private float rotationSpeed;       // Rotation speed around the center
    private float currentRadius;      // Current radius
    private float angle;              // Current angle

    // Method to initialize the Ice Ball
    public void Initialize(Transform center, float maxR, float minR, float changeSpeed, float rotateSpeed)
    {
        centerTransform = center;
        maxRadius = maxR;
        minRadius = minR;
        radiusChangeSpeed = changeSpeed;
        rotationSpeed = rotateSpeed;
        currentRadius = minRadius;    // Start at the minimum radius
        angle = 0;                    // Start angle
    }

    private void Update()
    {
        if (centerTransform == null) return;

        // Update the angle based on rotation speed
        angle += rotationSpeed * Time.deltaTime;
        angle %= 360; // Ensure angle wraps around after completing a full circle

        // Update the radius
        if (currentRadius < maxRadius && currentRadius <= minRadius)
            radiusChangeSpeed = Mathf.Abs(radiusChangeSpeed); // Ensure positive speed for increasing radius
        else if (currentRadius >= maxRadius)
            radiusChangeSpeed = -Mathf.Abs(radiusChangeSpeed); // Ensure negative speed for decreasing radius

        currentRadius += radiusChangeSpeed * Time.deltaTime;
        currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius); // Clamp radius within specified bounds

        // Calculate position
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * currentRadius;
        transform.position = centerTransform.position + offset;
    }
}
