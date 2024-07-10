using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the object around its local Z axis at the specified speed
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
