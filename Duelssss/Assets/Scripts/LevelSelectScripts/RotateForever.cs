using UnityEngine;

public class RotateForever : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float speed = 90f;     // Degrees per second
    public bool clockwise = true; // Direction

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        transform.Rotate(0f, 0f, speed * direction * Time.deltaTime);
    }
}
