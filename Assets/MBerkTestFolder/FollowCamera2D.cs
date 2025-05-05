using UnityEngine;

public class FollowCamera2D : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;                  // The target (player) to follow
    [SerializeField] private bool autoFindPlayer = true;        // Automatically find player at start
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;        // How smoothly the camera follows (lower = smoother)
    [SerializeField] private Vector3 offset = new Vector3(0, 1, -10); // Offset from target (z must be negative)
    
    [Header("Boundary Settings")]
    [SerializeField] private bool useBoundaries = false;        // Whether to constrain camera within boundaries
    [SerializeField] private float minX = -10f;                 // Left boundary
    [SerializeField] private float maxX = 10f;                  // Right boundary 
    [SerializeField] private float minY = -5f;                  // Bottom boundary
    [SerializeField] private float maxY = 5f;                   // Top boundary
    
    [Header("Look Ahead")]
    [SerializeField] private bool enableLookAhead = false;      // Enable camera to look ahead of movement direction
    [SerializeField] private float lookAheadDistance = 2f;      // How far to look ahead
    [SerializeField] private float lookAheadSpeed = 3f;         // How fast to adjust look ahead
    
    // Internal tracking variables
    private Vector3 velocity = Vector3.zero;
    private float lookAheadFactor = 0f;
    private float lookAheadDirection = 1f;
    private float lastTargetX = 0f;
    
    private void Start()
    {
        // If auto-find is enabled, look for player in scene
        if (autoFindPlayer && target == null)
        {
            // Try to find by tag first (most reliable)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            // If not found by tag, try to find by player controller component
            if (player == null)
                player = FindObjectOfType<TestPlayerControler>()?.gameObject;
                
            if (player != null)
                target = player.transform;
            else
                Debug.LogWarning("FollowCamera2D: Could not find player automatically. Please assign target manually.");
        }
        
        if (target != null)
            lastTargetX = target.position.x;
    }
    
    private void LateUpdate()
    {
        if (target == null)
            return;
            
        Vector3 desiredPosition = CalculateDesiredPosition();
        
        // Apply boundaries if enabled
        if (useBoundaries)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }
        
        // Smoothly move camera to desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        
        // Update last position for look ahead calculations
        lastTargetX = target.position.x;
    }
    
    private Vector3 CalculateDesiredPosition()
    {
        Vector3 basePosition = target.position + offset;
        
        // Apply look ahead effect if enabled
        if (enableLookAhead)
        {
            // Determine direction change
            float directionChange = Mathf.Sign(target.position.x - lastTargetX);
            
            // Only update look ahead direction when player actually moves and changes direction
            if (directionChange != 0 && directionChange != lookAheadDirection)
            {
                lookAheadDirection = directionChange;
                lookAheadFactor = 0; // Reset look ahead when changing direction
            }
            
            // Gradually increase look ahead factor
            lookAheadFactor = Mathf.Lerp(
                lookAheadFactor,
                (Mathf.Abs(target.position.x - lastTargetX) > 0.01f) ? 1 : 0,
                Time.deltaTime * lookAheadSpeed
            );
            
            // Apply look ahead to the position
            basePosition += Vector3.right * lookAheadDirection * lookAheadDistance * lookAheadFactor;
        }
        
        return basePosition;
    }
    
    // Gizmo to visualize camera boundaries in editor
    private void OnDrawGizmosSelected()
    {
        if (!useBoundaries)
            return;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY, 0));
    }
    
    // Public method to set target at runtime
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        lastTargetX = newTarget.position.x;
    }
}