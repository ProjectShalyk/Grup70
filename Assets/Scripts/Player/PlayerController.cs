using UnityEngine;
public partial class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 70f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private float apexGravityScale = 1.5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Coyote & Buffer")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.15f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpPressed;
    private float coyoteCounter;
    private float bufferCounter;
    private bool isGrounded;

    IInteractable currentInteractable;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        InteractUpdate();
        MovementUpdate();
    }

    private void FixedUpdate()
    {
        MovementFixedUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }




}
