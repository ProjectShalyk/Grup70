using UnityEngine;

public class TestPlayerControler : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator; // Uncomment if you have an animator
    private SpriteRenderer spriteRenderer;

    // States
    private bool isGrounded;
    private float horizontalInput;
    private bool jumpInput;
    private bool isFacingRight = true;

    // Ground check parameters
    [SerializeField] private LayerMask groundLayer;
    private float groundCheckExtraHeight = 0.05f;

    private void Awake()
    {
        // Get component references
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>(); // Uncomment if you have an animator
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up Rigidbody2D for 2D platformer
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        // Get player input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        // Check if player is grounded using the box collider
        isGrounded = IsGrounded();

        // Handle jump input
        if (jumpInput && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Handle character direction (flip sprite)
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Update animations (uncomment and modify as needed)
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        // Move the character
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Apply better jump physics
        if (rb.linearVelocity.y < 0)
        {
            // Falling - apply higher gravity
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Rising but jump button released - apply slightly higher gravity for shorter jumps
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private bool IsGrounded()
    {
        // Create a box cast below the player's collider to check for ground
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center, 
            boxCollider.bounds.size, 
            0f, 
            Vector2.down, 
            groundCheckExtraHeight, 
            groundLayer);
            
        return raycastHit.collider != null;
    }

    private void Flip()
    {
        // Flip the character's facing direction
        isFacingRight = !isFacingRight;
        
        // Method 1: Flip using scale
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        
        // Method 2: Flip using SpriteRenderer (comment out Method 1 if using this)
        // spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void UpdateAnimations()
    {
        // Only use this if you have an animator setup
        if (animator != null)
        {
            // Set animation parameters based on movement
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        }
    }
}