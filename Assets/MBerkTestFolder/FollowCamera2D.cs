using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // The target to follow (your player character)
    public Transform target;
    
    // Offset to maintain from the target (useful for adjusting camera position)
    public Vector3 offset = new Vector3(0, 0, -10);
    
    void LateUpdate()
    {
        // Check if we have a valid target
        if (target != null)
        {
            // Simply set the camera position to match the target position plus the offset
            // This provides immediate following with no smoothing
            transform.position = new Vector3(target.position.x, target.position.y, 0) + offset;
        }
    }
}