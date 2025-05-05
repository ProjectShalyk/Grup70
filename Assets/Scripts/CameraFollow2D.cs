using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;         // Takip edilecek nesne (örn. oyuncu)
    public float smoothSpeed = 0.125f;  // Takip hýzý
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Kamera ofseti

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
